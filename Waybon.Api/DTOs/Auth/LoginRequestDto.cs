using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Auth
{
    public class LoginRequestDto
    {
        private string _email = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email
        {
            get => _email;
            set => _email = value.ToLower();
        }

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}