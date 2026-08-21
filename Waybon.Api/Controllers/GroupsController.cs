using Microsoft.AspNetCore.Mvc;
using Waybon.Api.DTOs.Group;
using Waybon.Api.DTOs;
using Waybon.Application.Services;

namespace Waybon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController(GroupService groupService, AuthService authService) : ControllerBase
    {
        private readonly GroupService _groupService = groupService;
        private readonly AuthService _authService = authService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(CreateGroupRequestDto request)
        {
            var ownerUserId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _groupService.CreateGroupAsync(ownerUserId, request.Name);
            var response = new SuccessResponseDto
            {
                Success = true
            };

            return Ok(response);
        }

        [HttpPost("{groupId}/delete")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int groupId, SessionIdRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _groupService.DeleteGroupAsync(groupId, userId);
            var response = new SuccessResponseDto
            {
                Success = true
            };

            return Ok(response);
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinGroup(JoinGroupRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _groupService.JoinGroupAsync(userId, request.JoinCode);
            var response = new SuccessResponseDto
            {
                Success = true
            };

            return Ok(response);
        }

        [HttpPost("{groupId}/regenerate-code")]
        public async Task<IActionResult> RegenerateJoinCode([FromRoute] int groupId, SessionIdRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            var result = await _groupService.RegenerateJoinCodeAsync(groupId, userId);
            var response = new RegenerateJoinCodeResponseDto
            {
                JoinCode = result.JoinCode,
                JoinCodeExpiresAt = result.JoinCodeExpiresAt
            };

            return Ok(response);
        }

        [HttpPost("get-joined")]
        public async Task<IActionResult> GetGroupsByUserId(SessionIdRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            var groups = await _groupService.GetGroupsByUserIdAsync(userId);
            var response = groups.Select
            (
                g => new GroupSummaryDto
                {
                    GroupId = g.GroupId,
                    OwnerUserId = g.OwnerUserId,
                    Username = g.Username,
                    Name = g.Name,
                    JoinCode = g.JoinCode,
                    JoinCodeExpiresAt = g.JoinCodeExpiresAt,
                    CreatedAt = g.CreatedAt
                }
            );

            return Ok(response);
        }

        [HttpPost("{groupId}/get-members")]
        public async Task<IActionResult> GetGroupMembers([FromRoute] int groupId, SessionIdRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            var members = await _groupService.GetGroupMembersAsync(groupId, userId);
            var response = members.Select
            (
                m => new GroupMemberDto
                {
                    UserId = m.UserId,
                    Username = m.Username,
                    Rol = m.Rol,
                    SharingEnabled = m.SharingEnabled,
                    BlockedByMe = m.BlockedByMe,
                    BlockingMe = m.BlockingMe,
                    Latitude = m.Latitude,
                    Longitude = m.Longitude,
                    LocationUpdatedAt = m.LocationUpdatedAt,
                    LastActivityAt = m.LastActivityAt
                }
            );

            return Ok(response);
        }

        [HttpPost("{groupId}/leave")]
        public async Task<IActionResult> LeaveGroup([FromRoute] int groupId, SessionIdRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _groupService.LeaveGroupAsync(groupId, userId);
            var response = new SuccessResponseDto
            {
                Success = true
            };

            return Ok(response);
        }

        [HttpPost("{groupId}/kick-member")]
        public async Task<IActionResult> KickMember([FromRoute] int groupId, TargetMemberRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _groupService.KickMemberAsync(groupId, userId, request.TargetUserId);
            var response = new SuccessResponseDto
            {
                Success = true
            };

            return Ok(response);
        }
    }
}