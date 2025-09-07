using Hotelio.Booking.IntegrationEvents;

namespace Hotelio.Booking.Statistics.EventHandlers;

public interface IBookingCreatedEventHandler
{
    Task HandleAsync(BookingCreated @event, CancellationToken cancellationToken);
}