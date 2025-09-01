using System.Text.Json;
using Hotelio.Booking.IntegrationEvents;
using Confluent.Kafka;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public class BookingCreatedEventProducer : IBookingCreatedEventProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<BookingCreatedEventProducer> _logger;
    public const string TopicName = "booking-events";

    public BookingCreatedEventProducer(IProducer<string, string> producer, ILogger<BookingCreatedEventProducer> logger)
    {
        _producer = producer;
        _logger = logger;
    }
    
    public async Task Publish(BookingCreated @event, CancellationToken cancellationToken = default)
    {
        var message = new Message<string, string>
        {
            Key = nameof(BookingCreated),
            Value = JsonSerializer.Serialize(@event)
        };
        
        await _producer.ProduceAsync(TopicName, message, cancellationToken);
        _logger.LogInformation("Published event {EventType} id:{EventId}: {EventData}", nameof(BookingCreated), @event.EventId, @event);
    }
}