namespace Waybon.Api.DTOs.Auth
{
    public class LoginResponseDto
    {
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public bool SharingEnabled { get; set; }
    }
}