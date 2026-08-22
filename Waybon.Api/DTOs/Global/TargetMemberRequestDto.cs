using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Global
{
    public class TargetMemberRequestDto
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid TargetUserId { get; set; }
    }
}