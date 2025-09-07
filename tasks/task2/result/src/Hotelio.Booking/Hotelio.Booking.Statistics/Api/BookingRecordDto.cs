using Hotelio.Booking.Statistics.Entities;

namespace Hotelio.Booking.Statistics.Api;

public class BookingRecordDto(
    string bookingId,
    string userId,
    string hotelId,
    string? promoCode,
    double discountPercent,
    double price,
    DateTime createdAt)
{
    public string BookingId { get; private init; } = bookingId;
    public string UserId { get; private init; } = userId;
    public string HotelId { get; private init; } = hotelId;
    public string? PromoCode { get; private init; } = promoCode;
    public double DiscountPercent { get; private init; } = discountPercent;
    public double Price { get; private init; } = price;
    public DateTime CreatedAt { get; private init; } = createdAt;

    public static BookingRecordDto FromEntity(BookingRecord record)
    {
        return new BookingRecordDto(record.BookingId, record.UserId, record.HotelId, record.PromoCode, record.DiscountPercent, record.Price, record.CreatedAt);
    }
}