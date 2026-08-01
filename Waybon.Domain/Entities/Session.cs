namespace Waybon.Domain.Entities
{
    public class Session
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

        private DateTime _lastActivityAt;
        public DateTime LastActivityAt
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
    }
}