namespace Hotelio.Booking.Service.Integrations.Hotels;

public class HotelsClient : IHotelsClient
{
    private readonly HttpClient _httpClient;

    public HotelsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsOperationalAsync(string hotelId, CancellationToken cancellationToken = default)
    {
        const string path = "{0}/operational";
        var response = await _httpClient.GetAsync(new Uri(string.Format(path, hotelId), UriKind.Relative), cancellationToken);
        response.EnsureSuccessStatusCode();
        return bool.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
    }
    
    public async Task<bool> IsFullyBookedAsync(string hotelId, CancellationToken cancellationToken = default)
    {
        const string path = "{0}/fully-booked";
        var response = await _httpClient.GetAsync(new Uri(string.Format(path, hotelId), UriKind.Relative), cancellationToken);
        response.EnsureSuccessStatusCode();
        return bool.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
    }
}