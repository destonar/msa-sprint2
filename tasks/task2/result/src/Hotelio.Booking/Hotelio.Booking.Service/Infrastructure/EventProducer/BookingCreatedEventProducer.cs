using System.Text.Json;
using Hotelio.Booking.IntegrationEvents;
using Confluent.Kafka;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public class BookingCreatedEventProducer : IBookingCreatedEventProducer
{
    private readonly IProducer<string, string> _producer;
    public const string TopicName = "booking-events";

    public BookingCreatedEventProducer(IProducer<string, string> producer)
    {
        _producer = producer;
    }
    
    public async Task Publish(BookingCreated @event, CancellationToken cancellationToken = default)
    {
        var message = new Message<string, string>
        {
            Key = nameof(BookingCreated),
            Value = JsonSerializer.Serialize(@event)
        };
        
        await _producer.ProduceAsync(TopicName, message, cancellationToken);
    }
}