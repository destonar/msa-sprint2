namespace Hotelio.Booking.Service.Integrations.PromoCodes;

public class PromoCodeDto
{
    public string Code { get; init; }
    public double Discount { get; init; }
    public bool VipOnly { get; init; }
    public bool Expired { get; init; }
    public DateTime ValidUntil { get; init; }
    public string Description { get; init; }
}