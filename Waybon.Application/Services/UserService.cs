using Waybon.Application.Interfaces;
using Waybon.Domain.Interfaces;

namespace Waybon.Application.Services
{
    public class UserService(IUserRepository userRepository, ISharingExceptionRepository sharingExceptionRepository, IUserRefreshNotifier userNotifier)
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISharingExceptionRepository _sharingExceptionRepository = sharingExceptionRepository;
        private readonly IUserRefreshNotifier _userNotifier = userNotifier;

        public async Task BlockUserAsync(Guid sharerUserId, Guid blockedUserId)
        {
            if (!await _sharingExceptionRepository.CreateExceptionAsync(sharerUserId, blockedUserId))
            {
                throw new InvalidOperationException("Failed to block user.");
            }

            await _userNotifier.RefreshUserAsync(sharerUserId);
        }

        public async Task UnblockUserAsync(Guid sharerUserId, Guid blockedUserId)
        {
            if (!await _sharingExceptionRepository.RemoveExceptionAsync(sharerUserId, blockedUserId))
            {
                throw new InvalidOperationException("Failed to unblock user.");
            }

            await _userNotifier.RefreshUserAsync(sharerUserId);
        }

        public async Task UpdateSharingEnabledAsync(Guid userId, bool sharingEnabled)
        {
            if (!await _userRepository.UpdateSharingEnabledAsync(userId, sharingEnabled))
            {
                throw new InvalidOperationException("Failed to update the sharing preference.");
            }

            await _userNotifier.RefreshUserAsync(userId);
        }
    }
}