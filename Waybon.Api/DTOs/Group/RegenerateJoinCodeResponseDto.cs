namespace Waybon.Api.DTOs.Group
{
    public class RegenerateJoinCodeResponseDto
    {
        public string JoinCode { get; set; } = string.Empty;

        public DateTime JoinCodeExpiresAt { get; set; }
    }
}