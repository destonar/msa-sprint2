namespace Hotelio.Booking.Service.Entities;

public class Booking(
    string userId,
    string hotelId,
    string promoCode,
    double discountPercent,
    double price,
    DateTime createdAt)
{
    public long Id { get; private init; }
    public string UserId { get; private init; } = userId;
    public string HotelId { get; private init; } = hotelId;
    public string? PromoCode { get; private init; } = promoCode;
    public double DiscountPercent { get; private init; } = discountPercent;
    public double Price { get; private init; } = price;
    public DateTime CreatedAt { get; private init; } = createdAt;
}