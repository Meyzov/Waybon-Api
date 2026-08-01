namespace Waybon.Api.DTOs.Auth
{
    public class LoginResponseDto
    {
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

        private Guid _userId;
        public Guid UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
            }
        }

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
