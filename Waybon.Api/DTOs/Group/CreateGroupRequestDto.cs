using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Group
{
    public class CreateGroupRequestDto
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
    }
}