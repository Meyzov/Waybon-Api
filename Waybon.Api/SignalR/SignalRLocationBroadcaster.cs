using Microsoft.AspNetCore.SignalR;
using Waybon.Api.Hubs;
using Waybon.Application.Interfaces;

namespace Waybon.Api.SignalR
{
    public class SignalRLocationBroadcaster(IHubContext<LocationHub> hubContext) : ILocationBroadcaster
    {
        private readonly IHubContext<LocationHub> _hubContext = hubContext;

        public async Task SendLocationUpdateAsync(Guid userId, double latitude, double longitude, IEnumerable<string> connectionIds)
        {
            var ids = connectionIds.ToList();
            if (ids.Count == 0)
            {
                return;
            }

            await _hubContext.Clients.Clients(ids).SendAsync("LocationUpdated", userId, latitude, longitude);
        }
    }
}