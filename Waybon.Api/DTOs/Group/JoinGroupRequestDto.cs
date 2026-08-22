using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Group
{
    public class JoinGroupRequestDto
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string JoinCode { get; set; } = string.Empty;
    }
}