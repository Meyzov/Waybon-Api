using Waybon.Domain.Entities;

namespace Waybon.Domain.Interfaces
{
    public interface IGroupRepository
    {
        Task CreateGroupAsync(UserGroup userGroup);
        Task<UserGroup?> GetGroupByIdAsync(int groupId);
        Task<bool> DeleteGroupAsync(int groupId);
        Task<UserGroup?> GetGroupByJoinCodeAsync(string joinCode);
        Task<UserGroup?> RegenerateJoinCodeAsync(int groupId, string newJoinCode);
        Task<IEnumerable<UserGroup>> GetGroupsByUserIdAsync(Guid userId);
    }
}