namespace Waybon.Application.Interfaces
{
    public interface IConnectionManager
    {
        void AddConnection(string connectionId, Guid userId);
        void RemoveConnection(string connectionId);
        Guid? GetUserId(string connectionId);
        string? GetConnectionId(Guid userId);
    }
}