using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        private string _email = string.Empty;
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value.ToLower();
            }
        }

        [Required]
        [StringLength(255, MinimumLength = 6)]
        private string _password = string.Empty;
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }
    }
}
