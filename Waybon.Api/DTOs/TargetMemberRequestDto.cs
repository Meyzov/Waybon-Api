using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs
{
    public class TargetMemberRequestDto
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
        private Guid _targetUserId;
        public Guid TargetUserId
        {
            get
            {
                return _targetUserId;
            }

            set
            {
                _targetUserId = value;
            }
        }
    }
}
