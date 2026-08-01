namespace Waybon.Domain.Interfaces
{
    public interface ISharingExceptionRepository
    {
        Task<bool> CreateExceptionAsync(Guid sharerUserId, Guid blockedUserId);
        Task<bool> RemoveExceptionAsync(Guid sharerUserId, Guid blockedUserId);
    }
}