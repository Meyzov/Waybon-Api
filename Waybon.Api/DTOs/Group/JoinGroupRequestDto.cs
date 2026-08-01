using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Group
{
    public class JoinGroupRequestDto
    {
        [Required]
        private Guid _sessionId;
        public Guid SessionId
        {
            get
            {
                return _sessionId;
            }

            set
            {
                _sessionId = value;
            }
        }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        private string _joinCode = string.Empty;
        public string JoinCode
        {
            get
            {
                return _joinCode;
            }

            set
            {
                _joinCode = value;
            }
        }
    }
}
