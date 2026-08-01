namespace Waybon.Domain.Entities
{
    public class GroupMember
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

        private DateTime _joinedAt;
        public DateTime JoinedAt
        {
            get
            {
                return _joinedAt;
            }

            set
            {
                _joinedAt = value;
            }
        }
    }
}