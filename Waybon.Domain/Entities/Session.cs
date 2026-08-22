namespace Waybon.Domain.Entities
{
    public class Session
    {
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastActivityAt { get; set; }
    }
}