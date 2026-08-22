namespace Waybon.Domain.Entities
{
    public class SharingException
    {
        public Guid SharerUserId { get; set; }

        public Guid BlockedUserId { get; set; }
    }
}