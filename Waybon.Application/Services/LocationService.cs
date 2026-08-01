using Waybon.Application.Helpers;
using Waybon.Application.Interfaces;
using Waybon.Domain.Interfaces;

namespace Waybon.Application.Services
{
    public class LocationService(IGroupMemberRepository groupMemberRepository, IUserRepository userRepository, IConnectionManager connectionManager, ILocationBroadcaster locationBroadcaster, ILocationCache locationCache) : IGroupMembershipNotifier, IUserRefreshNotifier
    {
        private readonly IGroupMemberRepository _groupMemberRepository = groupMemberRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConnectionManager _connectionManager = connectionManager;
        private readonly ILocationBroadcaster _locationBroadcaster = locationBroadcaster;
        private readonly ILocationCache _locationCache = locationCache;

        public async Task ProcessLocationUpdateAsync(Guid userId, double latitude, double longitude)
        {
            if (!_locationCache.IsSharing(userId))
            {
                return;
            }

            var recipientIds = _locationCache.GetRecipients(userId);
            if (!recipientIds.Any())
            {
                return;
            }

            var connectionIds = new List<string>();
            foreach (var recipientId in recipientIds)
            {
                var connId = _connectionManager.GetConnectionId(recipientId);
                if (connId != null)
                {
                    connectionIds.Add(connId);
                }
            }

            if (connectionIds.Count == 0)
            {
                return;
            }

            await _locationBroadcaster.SendLocationUpdateAsync(userId, latitude, longitude, connectionIds);
        }

        public async Task WarmupUserAsync(Guid userId)
        {
            var sharing = await _userRepository.IsSharingEnabledAsync(userId);
            _locationCache.SetSharing(userId, sharing);

            if (sharing)
            {
                var recipients = await _groupMemberRepository.GetRecipientsForUserAsync(userId);
                _locationCache.SetRecipients(userId, recipients);
            }
        }

        public async Task RefreshUserAsync(Guid userId)
        {
            await WarmupUserAsync(userId);
        }

        public async Task OnMembershipChangedAsync(int groupId)
        {
            var memberIds = await _groupMemberRepository.GetMembersUserIdsAsync(groupId);
            foreach (var memberId in memberIds)
            {
                await RefreshUserAsync(memberId);
            }
        }

        public void RemoveUserCache(Guid userId)
        {
            _locationCache.RemoveUser(userId);
        }
    }
}