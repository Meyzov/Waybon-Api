using System.ComponentModel.DataAnnotations;

namespace Waybon.Api.DTOs.Group
{
    public class CreateGroupRequestDto
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
        [StringLength(100, MinimumLength = 1)]
        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
    }
}
