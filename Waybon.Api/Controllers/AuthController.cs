using Microsoft.AspNetCore.Mvc;
using Waybon.Api.DTOs;
using Waybon.Api.DTOs.Auth;
using Waybon.Api.DTOs.Global;
using Waybon.Application.Services;

namespace Waybon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        private readonly AuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            await _authService.RegisterAsync(request.Username, request.Email, request.Password);
            return Ok(new SuccessResponseDto());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);
            var response = new LoginResponseDto
            {
                SessionId = result.SessionId,
                UserId = result.UserId,
                Username = result.Username,
                RoleName = result.RoleName,
                SharingEnabled = result.SharingEnabled
            };

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(SessionIdRequestDto request)
        {
            await _authService.LogoutAsync(request.SessionId);
            return Ok(new SuccessResponseDto());
        }
    }
}