using Waybon.Domain.Entities;

namespace Waybon.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(AppUser user);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<bool> UpdateLoginAttemptAsync(Guid userId, int failedLoginAttempts, DateTime? lockedUntil);
        Task<bool> UpdateSharingEnabledAsync(Guid userId, bool sharingEnabled);
        Task<bool> IsSharingEnabledAsync(Guid userId);
    }
}