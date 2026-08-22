using Waybon.Application.Helpers;
using Waybon.Domain.Entities;
using Waybon.Domain.Interfaces;

namespace Waybon.Application.Services
{
    public class AuthService(IUserRepository userRepository, IRoleRepository roleRepository, ISessionRepository sessionRepository)
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task RegisterAsync(string username, string email, string password)
        {
            var passwordHash = PasswordHasher.Hash(password);
            var newUser = new AppUser
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            if (!await _userRepository.CreateUserAsync(newUser))
            {
                throw new InvalidOperationException("User registration failed due to a database error.");
            }
        }

        public async Task<LoginDetails> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email) ?? throw new UnauthorizedAccessException("Invalid email or password.");
            if (user.LockedUntil != null && user.LockedUntil > DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException($"Account is temporarily locked. Please try again after {user.LockedUntil:HH:mm} UTC.");
            }

            var isPasswordValid = PasswordHasher.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                user.FailedLoginAttempts += 1;
                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockedUntil = DateTime.UtcNow.AddMinutes(15);
                }
            }
            else
            {
                user.FailedLoginAttempts = 0;
                user.LockedUntil = null;
            }

            if (!await _userRepository.UpdateLoginAttemptAsync(user.UserId, user.FailedLoginAttempts, user.LockedUntil))
            {
                throw new InvalidOperationException("Unable to update login attempts.");
            }

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var session = await _sessionRepository.CreateOrReplaceSessionAsync(user.UserId);
            var role = await _roleRepository.GetRoleByIdAsync(user.RoleId) ?? throw new InvalidOperationException("Role not found.");

            return new LoginDetails
            {
                SessionId = session.SessionId,
                UserId = session.UserId,
                Username = user.Username,
                RoleName = role.Name,
                SharingEnabled = user.SharingEnabled
            };
        }

        public async Task LogoutAsync(Guid sessionId)
        {
            if (!await _sessionRepository.DeleteSessionAsync(sessionId))
            {
                throw new InvalidOperationException("Session not found.");
            }
        }

        public async Task<Guid> GetUserIdFromSessionAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetSessionByIdAsync(sessionId) ?? throw new UnauthorizedAccessException("Session not found.");
            return session.UserId;
        }
    }
}