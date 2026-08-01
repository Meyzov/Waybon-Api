using Waybon.Application.Helpers;
using Waybon.Application.Interfaces;
using Waybon.Domain.Entities;
using Waybon.Domain.Interfaces;

namespace Waybon.Application.Services
{
    public class GroupService(IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository, IGroupMembershipNotifier groupNotifier, IUserRefreshNotifier userNotifier)
    {
        private readonly IGroupRepository _groupRepository = groupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository = groupMemberRepository;
        private readonly IGroupMembershipNotifier _groupNotifier = groupNotifier;
        private readonly IUserRefreshNotifier _userNotifier = userNotifier;

        public async Task CreateGroupAsync(Guid ownerUserId, string name)
        {
            var joinCode = JoinCodeGenerator.Generate();
            var newGroup = new UserGroup
            {
                OwnerUserId = ownerUserId,
                Name = name,
                JoinCode = joinCode
            };

            await _groupRepository.CreateGroupAsync(newGroup); // the database has a trigger to enforce group creation limit per role
        }

        public async Task DeleteGroupAsync(int groupId, Guid requestingUserId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId) ?? throw new InvalidOperationException("Group not found.");
            if (group.OwnerUserId != requestingUserId)
            {
                throw new UnauthorizedAccessException("Only the group owner can delete this group.");
            }

            var memberIds = await _groupMemberRepository.GetMembersUserIdsAsync(groupId);

            if (!await _groupRepository.DeleteGroupAsync(groupId)) // the database has on delete cascade
            {
                throw new InvalidOperationException("Failed to delete the group.");
            }

            foreach (var memberId in memberIds)
            {
                await _userNotifier.RefreshUserAsync(memberId);
            }
        }

        public async Task JoinGroupAsync(Guid userId, string joinCode)
        {
            var group = await _groupRepository.GetGroupByJoinCodeAsync(joinCode) ?? throw new InvalidOperationException("Invalid join code.");
            if (group.JoinCodeExpiresAt < DateTime.UtcNow)
            {
                throw new InvalidOperationException("This join code has expired.");
            }

            if (!await _groupMemberRepository.AddMemberAsync(group.GroupId, userId)) // the database has groupId and userId as the primary key for group members
            {
                throw new InvalidOperationException("Failed to join the group");
            }

            await _groupNotifier.OnMembershipChangedAsync(group.GroupId);
        }

        public async Task<UserGroup> RegenerateJoinCodeAsync(int groupId, Guid requestingUserId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId) ?? throw new InvalidOperationException("Group not found.");
            if (group.OwnerUserId != requestingUserId)
            {
                throw new UnauthorizedAccessException("Only the group owner can regenerate the join code.");
            }

            var newJoinCode = JoinCodeGenerator.Generate();
            return await _groupRepository.RegenerateJoinCodeAsync(groupId, newJoinCode) ?? throw new InvalidOperationException("Failed to regenerate the join code.");
        }

        public async Task<IEnumerable<UserGroup>> GetGroupsByUserIdAsync(Guid userId)
        {
            return await _groupRepository.GetGroupsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<GroupMemberDetail>> GetGroupMembersAsync(int groupId, Guid userId)
        {
            if (!await _groupMemberRepository.IsMemberAsync(groupId, userId))
            {
                throw new UnauthorizedAccessException("You are not a member of this group.");
            }

            return await _groupMemberRepository.GetMembersByGroupIdAsync(groupId, userId);
        }

        public async Task LeaveGroupAsync(int groupId, Guid userId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId) ?? throw new InvalidOperationException("Group not found.");
            if (group.OwnerUserId == userId)
            {
                throw new InvalidOperationException("The owner cannot leave the group.");
            }

            if (!await _groupMemberRepository.RemoveMemberAsync(groupId, userId))
            {
                throw new InvalidOperationException("Failed to leave the group.");
            }

            await _groupNotifier.OnMembershipChangedAsync(groupId);
            await _userNotifier.RefreshUserAsync(userId);
        }

        public async Task KickMemberAsync(int groupId, Guid requestingUserId, Guid targetUserId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId) ?? throw new InvalidOperationException("Group not found.");
            if (group.OwnerUserId != requestingUserId)
            {
                throw new UnauthorizedAccessException("Only the group owner can remove members.");
            }

            if (targetUserId == requestingUserId)
            {
                throw new InvalidOperationException("The owner cannot remove themselves from the group.");
            }

            if (!await _groupMemberRepository.RemoveMemberAsync(groupId, targetUserId))
            {
                throw new InvalidOperationException("Failed to kick member from the group");
            }

            await _groupNotifier.OnMembershipChangedAsync(group.GroupId);
            await _userNotifier.RefreshUserAsync(targetUserId);
        }
    }
}