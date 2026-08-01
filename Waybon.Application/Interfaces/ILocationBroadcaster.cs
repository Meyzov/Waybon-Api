namespace Waybon.Application.Interfaces
{
    public interface ILocationBroadcaster
    {
        Task SendLocationUpdateAsync(Guid userId, double latitude, double longitude, IEnumerable<string> connectionIds);
    }
}