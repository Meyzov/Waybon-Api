using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.User
{
    public class UpdateSharingRequestDto
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
        private bool _sharingEnabled;
        public bool SharingEnabled
        {
            get
            {
                return _sharingEnabled;
            }

            set
            {
                _sharingEnabled = value;
            }
        }
    }
}