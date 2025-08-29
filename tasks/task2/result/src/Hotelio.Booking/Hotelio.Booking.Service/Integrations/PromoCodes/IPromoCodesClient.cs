namespace Hotelio.Booking.Service.Integrations.PromoCodes;

public interface IPromoCodesClient
{
    Task<PromoCodeDto?> ValidateCode(string code, string userId, CancellationToken cancellationToken = default);
}