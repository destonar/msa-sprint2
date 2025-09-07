namespace Hotelio.Booking.Service.Integrations.Users;

public class UsersClient : IUsersClient
{
    private readonly HttpClient _httpClient;

    public UsersClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> IsUserActiveAsync(string userId, CancellationToken cancellationToken = default)
    {
        const string path = "{0}/active";
        var response = await _httpClient.GetAsync(new Uri(string.Format(path, userId), UriKind.Relative), cancellationToken);
        response.EnsureSuccessStatusCode();
        return bool.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
    }
    
    public async Task<bool> IsUserBlacklistedAsync(string userId, CancellationToken cancellationToken = default)
    {
        const string path = "{0}/blacklisted";
        var response = await _httpClient.GetAsync(new Uri(string.Format(path, userId), UriKind.Relative), cancellationToken);
        response.EnsureSuccessStatusCode();
        return bool.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
    }

    public async Task<string?> GetUserStatusAsync(string userId, CancellationToken cancellationToken = default)
    {
        const string path = "{0}/status";
        var response = await _httpClient.GetAsync(new Uri(string.Format(path, userId), UriKind.Relative), cancellationToken);
        response.EnsureSuccessStatusCode();
        var status = await response.Content.ReadAsStringAsync(cancellationToken);
        return status == string.Empty ? null : status;
    }
}