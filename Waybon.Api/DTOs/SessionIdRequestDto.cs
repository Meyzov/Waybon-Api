using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs
{
    public class SessionIdRequestDto
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
    }
}
