namespace Waybon.Api.DTOs.Group
{
    public class RegenerateJoinCodeResponseDto
    {
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

        private DateTime _joinCodeExpiresAt;
        public DateTime JoinCodeExpiresAt
        {
            get
            {
                return _joinCodeExpiresAt;
            }

            set
            {
                _joinCodeExpiresAt = value;
            }
        }
    }
}
