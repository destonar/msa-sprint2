namespace Hotelio.Booking.Service.Integrations.Reviews;

public interface IReviewsClient
{
    Task<bool> IsHotelTrustedAsync(string hotelId, CancellationToken cancellationToken = default);
}