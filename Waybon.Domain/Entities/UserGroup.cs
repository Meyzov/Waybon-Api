namespace Waybon.Domain.Entities
{
    public class UserGroup
    {
        private int _groupId;
        public int GroupId
        {
            get
            {
                return _groupId;
            }

            set
            {
                _groupId = value;
            }
        }

        private Guid _ownerUserId;
        public Guid OwnerUserId
        {
            get
            {
                return _ownerUserId;
            }

            set
            {
                _ownerUserId = value;
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

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get
            {
                return _createdAt;
            }

            set
            {
                _createdAt = value;
            }
        }
    }
}