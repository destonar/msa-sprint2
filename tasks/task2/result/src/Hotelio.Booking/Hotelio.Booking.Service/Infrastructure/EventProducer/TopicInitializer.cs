using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public class TopicInitializer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly EventProducerOptions _options;
    private readonly ILogger<TopicInitializer> _logger;

    public TopicInitializer(IConfiguration configuration, EventProducerOptions options, ILogger<TopicInitializer> logger)
    {
        _configuration = configuration;
        _options = options;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.IsEnabled)
        {
            _logger.LogInformation("Kafka publication is disabled via feature flag");
        }
        
        return;
        
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