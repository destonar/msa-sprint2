namespace Hotelio.Booking.Service.Integrations.Users;

public interface IUsersClient
{
    Task<bool> IsUserActiveAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserBlacklistedAsync(string userId, CancellationToken cancellationToken = default);
    Task<string?> GetUserStatusAsync(string userId, CancellationToken cancellationToken = default);
}