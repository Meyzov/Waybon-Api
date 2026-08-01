using Waybon.Domain.Entities;

namespace Waybon.Domain.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session> CreateOrReplaceSessionAsync(Guid userId);
        Task<bool> DeleteSessionAsync(Guid sessionId);
        Task<Session?> GetSessionByIdAsync(Guid sessionId);
    }
}