using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        private string _username = string.Empty;
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }

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

        [Required]
        [StringLength(50)]
        private string _roleName = string.Empty;
        public string RoleName
        {
            get
            {
                return _roleName;
            }

            set
            {
                _roleName = value;
            }
        }
    }
}
