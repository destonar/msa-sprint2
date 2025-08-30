using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public class TopicInitializer : BackgroundService
{
    private readonly IConfiguration _configuration;

    public TopicInitializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cs = _configuration.GetConnectionString("kafka");
        if (cs == null)
        {
            throw new InvalidOperationException("Kafka connection string not found");
        }
        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = cs }).Build();
        var metadata = adminClient.GetMetadata(TimeSpan.FromMilliseconds(500));
        if (metadata != null && metadata.Topics.Any(t => t.Topic == BookingCreatedEventProducer.TopicName && !t.Error.IsError))
        {
            return;
        }
        
        await adminClient.CreateTopicsAsync([
            new TopicSpecification { Name = BookingCreatedEventProducer.TopicName, ReplicationFactor = 1, NumPartitions = 1 }
        ]);
    }
}