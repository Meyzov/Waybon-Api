namespace Waybon.Domain.Entities
{
    public class AppUser
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime? LockedUntil { get; set; }

        public bool SharingEnabled { get; set; }
    }
}