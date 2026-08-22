using Microsoft.AspNetCore.Mvc;
using Waybon.Api.DTOs.Global;
using Waybon.Api.DTOs.User;
using Waybon.Application.Services;

namespace Waybon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService userService, AuthService authService) : ControllerBase
    {
        private readonly UserService _userService = userService;
        private readonly AuthService _authService = authService;

        [HttpPost("block")]
        public async Task<IActionResult> BlockUser(TargetMemberRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _userService.BlockUserAsync(userId, request.TargetUserId);
            return Ok(new SuccessResponseDto());
        }

        [HttpPost("unblock")]
        public async Task<IActionResult> UnblockUser(TargetMemberRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _userService.UnblockUserAsync(userId, request.TargetUserId);
            return Ok(new SuccessResponseDto());
        }

        [HttpPost("sharing")]
        public async Task<IActionResult> UpdateSharing(UpdateSharingRequestDto request)
        {
            var userId = await _authService.GetUserIdFromSessionAsync(request.SessionId);
            await _userService.UpdateSharingEnabledAsync(userId, request.SharingEnabled);
            return Ok(new SuccessResponseDto());
        }
    }
}