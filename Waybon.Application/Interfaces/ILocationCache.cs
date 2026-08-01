namespace Waybon.Application.Interfaces
{
    public interface ILocationCache
    {
        bool IsSharing(Guid userId);
        void SetSharing(Guid userId, bool sharing);
        IEnumerable<Guid> GetRecipients(Guid userId);
        void SetRecipients(Guid userId, IEnumerable<Guid> ids);
        void RemoveUser(Guid userId);
    }
}