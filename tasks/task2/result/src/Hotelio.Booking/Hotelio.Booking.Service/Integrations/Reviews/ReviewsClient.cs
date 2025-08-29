namespace Hotelio.Booking.Service.Integrations.Reviews;

public class ReviewsClient : IReviewsClient
{
    private readonly HttpClient _httpClient;

    public ReviewsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> IsHotelTrustedAsync(string hotelId, CancellationToken cancellationToken = default)
    {
        const string path = "hotel/{0}/trusted";
        var response = await _httpClient.GetAsync(new Uri(string.Format(path, hotelId), UriKind.Relative), cancellationToken);
        response.EnsureSuccessStatusCode();
        return bool.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
    }
    
}