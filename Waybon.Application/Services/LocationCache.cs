using System.Collections.Concurrent;
using Waybon.Application.Dtos.Metrics;
using Waybon.Application.Interfaces;

namespace Waybon.Application.Services
{
    public class LocationCache : ILocationCache, ICacheMetrics
    {
        private readonly ConcurrentDictionary<Guid, bool> _sharing = new();
        private readonly ConcurrentDictionary<Guid, HashSet<Guid>> _recipients = new();

        private static readonly HashSet<Guid> EmptySet = [];

        public bool IsSharing(Guid userId) => _sharing.GetValueOrDefault(userId);

        public void SetSharing(Guid userId, bool sharing) => _sharing[userId] = sharing;

        public IEnumerable<Guid> GetRecipients(Guid userId) => _recipients.GetValueOrDefault(userId) ?? EmptySet;

        public void SetRecipients(Guid userId, IEnumerable<Guid> ids) => _recipients[userId] = [.. ids];

        public void RemoveUser(Guid userId)
        {
            _sharing.TryRemove(userId, out _);
            _recipients.TryRemove(userId, out _);
        }

        public CacheMetricsDto GetMetrics()
        {
            long sharingBytes = _sharing.Count * 48;
            long recipientsBytes = 0;
            int totalRecipientGuids = 0;

            foreach (var kvp in _recipients)
            {
                totalRecipientGuids += kvp.Value.Count;
                recipientsBytes += 48 + (kvp.Value.Count * 48);
            }

            return new CacheMetricsDto
            (
                _sharing.Count,
                _sharing.Values.Count(v => v),
                _recipients.Count,
                totalRecipientGuids,
                Math.Round((sharingBytes + recipientsBytes) / 1024.0, 2)
            );
        }
    }
}
