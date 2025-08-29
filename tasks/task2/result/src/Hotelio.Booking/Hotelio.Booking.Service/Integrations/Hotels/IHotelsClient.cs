namespace Hotelio.Booking.Service.Integrations.Hotels;

public interface IHotelsClient
{
    Task<bool> IsOperationalAsync(string hotelId, CancellationToken cancellationToken = default);
    Task<bool> IsFullyBookedAsync(string hotelId, CancellationToken cancellationToken = default);
}