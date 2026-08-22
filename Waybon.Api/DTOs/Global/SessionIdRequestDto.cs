using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Global
{
    public class SessionIdRequestDto
    {
        [Required]
        public Guid SessionId { get; set; }
    }
}