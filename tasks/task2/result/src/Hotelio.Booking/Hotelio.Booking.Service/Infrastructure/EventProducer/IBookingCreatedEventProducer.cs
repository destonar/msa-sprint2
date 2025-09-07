using Hotelio.Booking.IntegrationEvents;

namespace Hotelio.Booking.Service.Infrastructure.EventProducer;

public interface IBookingCreatedEventProducer
{
    Task Publish(BookingCreated @event, CancellationToken cancellationToken = default);
}