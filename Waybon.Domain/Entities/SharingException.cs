namespace Waybon.Domain.Entities
{
    public class SharingException
    {
        private Guid _sharerUserId;
        public Guid SharerUserId
        {
            get
            {
                return _sharerUserId;
            }

            set
            {
                _sharerUserId = value;
            }
        }

        private Guid _blockedUserId;
        public Guid BlockedUserId
        {
            get
            {
                return _blockedUserId;
            }

            set
            {
                _blockedUserId = value;
            }
        }
    }
}