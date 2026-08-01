namespace Waybon.Domain.Entities
{
    public class AppUser
    {
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

        private string _email = string.Empty;
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
            }
        }

        private string _passwordHash = string.Empty;
        public string PasswordHash
        {
            get
            {
                return _passwordHash;
            }

            set
            {
                _passwordHash = value;
            }
        }

        private int _roleId;
        public int RoleId
        {
            get
            {
                return _roleId;
            }

            set
            {
                _roleId = value;
            }
        }

        private int _failedLoginAttempts;
        public int FailedLoginAttempts
        {
            get
            {
                return _failedLoginAttempts;
            }

            set
            {
                _failedLoginAttempts = value;
            }
        }

        private DateTime? _lockedUntil;
        public DateTime? LockedUntil
        {
            get
            {
                return _lockedUntil;
            }

            set
            {
                _lockedUntil = value;
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