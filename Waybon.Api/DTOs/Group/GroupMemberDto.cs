namespace Waybon.Api.DTOs.Group
{
    public class GroupMemberDto
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

        private string _rolname = string.Empty;
        public string Rolname
        {
            get
            {
                return _rolname;
            }

            set
            {
                _rolname = value;
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

        private bool _blockedByMe;
        public bool BlockedByMe
        {
            get
            {
                return _blockedByMe;
            }

            set
            {
                _blockedByMe = value;
            }
        }

        private bool _blockingMe;
        public bool BlockingMe
        {
            get
            {
                return _blockingMe;
            }

            set
            {
                _blockingMe = value;
            }
        }

        private DateTime? _lastActivityAt;
        public DateTime? LastActivityAt
        {
            get
            {
                return _lastActivityAt;
            }

            set
            {
                _lastActivityAt = value;
            }
        }

        private double? _latitude;
        public double? Latitude
        {
            get
            {
                return _latitude;
            }

            set
            {
                _latitude = value;
            }
        }

        private double? _longitude;
        public double? Longitude
        {
            get
            {
                return _longitude;
            }

            set
            {
                _longitude = value;
            }
        }

        private DateTime? _locationUpdatedAt;
        public DateTime? LocationUpdatedAt
        {
            get
            {
                return _locationUpdatedAt;
            }

            set
            {
                _locationUpdatedAt = value;
            }
        }
    }
}
