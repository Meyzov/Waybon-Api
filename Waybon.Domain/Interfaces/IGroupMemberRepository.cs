using Waybon.Domain.Entities;

namespace Waybon.Domain.Interfaces
{
    public interface IGroupMemberRepository
    {
        Task<bool> AddMemberAsync(int groupId, Guid userId);
        Task<bool> IsMemberAsync(int groupId, Guid userId);
        Task<IEnumerable<GroupMemberDetail>> GetMembersByGroupIdAsync(int groupId, Guid requestingUserId);
        Task<bool> RemoveMemberAsync(int groupId, Guid userId);
        Task<IEnumerable<Guid>> GetRecipientsForUserAsync(Guid userId);
        Task<IEnumerable<Guid>> GetMembersUserIdsAsync(int groupId);
    }
}