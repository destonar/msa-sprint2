using System.Text.Json;
using Hotelio.Booking.IntegrationEvents;
using Confluent.Kafka;
using Hotelio.Booking.Statistics.EventHandlers;

namespace Hotelio.Booking.Statistics;

public class BookingEventsWorker : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    private const string TopicName = "booking-events";
    
    public BookingEventsWorker(IConsumer<string, string> consumer,  IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var task = Task.Factory.StartNew(RunAsync, stoppingToken, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        if (task.IsCompleted)
        {
            return task;
        }
        
        return Task.CompletedTask;
    }

    private async Task RunAsync(object? ctObj)
    {
        var stoppingToken = (CancellationToken)ctObj!;
        var bootstrapServers = _configuration.GetConnectionString("kafka");
        if (bootstrapServers == null)
        {
            return;
        }

        if (!TopicExists(bootstrapServers))
        {
            return;
        }

        _consumer.Subscribe(TopicName);
        while (!stoppingToken.IsCancellationRequested)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            var result = _consumer.Consume(linked.Token);
            if (result.Message.Key != nameof(BookingCreated))
            {
                continue;
            }
            
            var @event = JsonSerializer.Deserialize<BookingCreated>(result.Message.Value);
            if (@event == null)
            {
                continue;
            }
            
            using var scope = _scopeFactory.CreateScope();
            var handler =  scope.ServiceProvider.GetRequiredService<IBookingCreatedEventHandler>();
            await handler.HandleAsync(@event, linked.Token);
            _consumer.Commit(result);
        }
    }

    private static bool TopicExists(string bootstrapServers)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        var metadata = adminClient.GetMetadata(TimeSpan.FromMilliseconds(500));
        return metadata != null && metadata.Topics.Any(t => t.Topic == TopicName && !t.Error.IsError);
    }
}