using System.Globalization;
using Booking;

namespace Hotelio.Booking.Service.Entities;

public static class BookingExtensions
{
    public static BookingResponse ToResponse(this Booking booking)
    {
        return new BookingResponse
        {
            Id = booking.Id.ToString(),
            UserId = booking.UserId,
            HotelId = booking.HotelId,
            PromoCode = booking.PromoCode ?? "",
            DiscountPercent = booking.DiscountPercent,
            Price = booking.Price,
            CreatedAt = booking.CreatedAt.ToString("o", CultureInfo.InvariantCulture)
        };
    }
}