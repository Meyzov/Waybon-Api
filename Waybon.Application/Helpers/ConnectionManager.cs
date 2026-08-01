using System.Collections.Concurrent;
using Waybon.Application.Dtos.Metrics;
using Waybon.Application.Interfaces;

namespace Waybon.Application.Helpers
{
    public class ConnectionManager : IConnectionManager, IConnectionMetrics
    {
        private readonly ConcurrentDictionary<string, Guid> _connectionToUser = new();
        private readonly ConcurrentDictionary<Guid, string> _userToConnection = new();

        public void AddConnection(string connectionId, Guid userId)
        {
            if (_userToConnection.TryRemove(userId, out var oldConnectionId))
            {
                _connectionToUser.TryRemove(oldConnectionId, out _);
            }

            _connectionToUser[connectionId] = userId;
            _userToConnection[userId] = connectionId;
        }

        public void RemoveConnection(string connectionId)
        {
            if (_connectionToUser.TryRemove(connectionId, out var userId))
            {
                _userToConnection.TryRemove(new KeyValuePair<Guid, string>(userId, connectionId));
            }
        }

        public Guid? GetUserId(string connectionId)
        {
            return _connectionToUser.TryGetValue(connectionId, out var userId) ? userId : null;
        }

        public string? GetConnectionId(Guid userId)
        {
            return _userToConnection.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }

        public ConnectionMetricsDto GetMetrics()
        {
            long connToUserBytes = _connectionToUser.Count * 120;
            long userToConnBytes = _userToConnection.Count * 120;

            return new ConnectionMetricsDto
            (
                _connectionToUser.Count,
                _userToConnection.Count,
                Math.Round((connToUserBytes + userToConnBytes) / 1024.0, 2)
            );
        }
    }
}