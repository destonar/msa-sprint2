namespace Hotelio.Booking.IntegrationEvents;

public record BookingCreated
{
    public BookingCreated(
        string bookingId,
        string userId,
        string hotelId,
        string? promoCode,
        double discountPercent,
        double price,
        DateTime occuredOn)
    {
        EventId = Guid.NewGuid();
        BookingId = bookingId;
        UserId = userId;
        HotelId = hotelId;
        PromoCode = promoCode;
        DiscountPercent = discountPercent;
        Price = price;
        OccuredOn = occuredOn;
    }

    public Guid EventId { get; init; }
    public string BookingId { get; init; }
    public string UserId { get; init; }
    public string HotelId { get; init; }
    public string? PromoCode { get; init; }
    public double DiscountPercent { get; init; }
    public double Price { get; init; }
    public DateTime OccuredOn { get; init; }
}