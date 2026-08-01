using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.User
{
    public class BlockUserRequestDto
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
        private Guid _blockedUserId;
        public Guid BlockedUserId
        {
            get
            {
                return _blockedUserId;
            }

            set
            {
                _blockedUserId = value;
            }
        }
    }
}
