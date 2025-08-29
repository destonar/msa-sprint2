using System.Text.Json;
using System.Web;

namespace Hotelio.Booking.Service.Integrations.PromoCodes;

public class PromoCodesClient : IPromoCodesClient
{
    private readonly HttpClient _httpClient;

    public PromoCodesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PromoCodeDto?> ValidateCode(string code, string userId, CancellationToken cancellationToken = default)
    {
        const string path = "validate?{0}";
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["code"] = code;
        query["userId"] = userId;
        
        var response = await _httpClient.PostAsync(new Uri(string.Format(path, query), UriKind.Relative), null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<PromoCodeDto>(await response.Content.ReadAsStreamAsync(cancellationToken), JsonSerializerOptions.Web);
    }
}