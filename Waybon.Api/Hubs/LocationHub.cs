using Microsoft.AspNetCore.SignalR;
using Waybon.Application.Interfaces;
using Waybon.Application.Services;

namespace Waybon.Api.Hubs
{
    public class LocationHub(AuthService authService, IConnectionManager connectionManager, LocationService locationService) : Hub
    {
        private readonly AuthService _authService = authService;
        private readonly IConnectionManager _connectionManager = connectionManager;
        private readonly LocationService _locationService = locationService;

        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                return;
            }

            var sessionIdString = context.Request.Query["sessionId"];
            if (string.IsNullOrEmpty(sessionIdString) || !Guid.TryParse(sessionIdString, out var sessionId))
            {
                Context.Abort();
                return;
            }

            try
            {
                var userId = await _authService.GetUserIdFromSessionAsync(sessionId);
                _connectionManager.AddConnection(Context.ConnectionId, userId);
                await _locationService.WarmupUserAsync(userId);
            }
            catch (UnauthorizedAccessException)
            {
                Context.Abort();
                return;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _connectionManager.GetUserId(Context.ConnectionId);
            _connectionManager.RemoveConnection(Context.ConnectionId);

            if (userId != null && _connectionManager.GetConnectionId(userId.Value) == null)
            {
                _locationService.RemoveUserCache(userId.Value);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendLocation(double latitude, double longitude)
        {
            var userId = _connectionManager.GetUserId(Context.ConnectionId);
            if (userId == null)
            {
                return;
            }

            await _locationService.ProcessLocationUpdateAsync(userId.Value, latitude, longitude);
        }
    }
}