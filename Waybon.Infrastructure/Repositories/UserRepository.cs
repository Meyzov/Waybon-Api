using Dapper;
using Waybon.Domain.Entities;
using Waybon.Domain.Interfaces;

namespace Waybon.Infrastructure.Repositories
{
    public class UserRepository(IDbConnectionFactory dbConnectionFactory) : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<bool> CreateUserAsync(AppUser user)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                INSERT INTO app_user (username, email, password_hash)
                VALUES (@Username, @Email, @PasswordHash)

            """
            ;
            parameters.Add("Username", user.Username);
            parameters.Add("Email", user.Email);
            parameters.Add("PasswordHash", user.PasswordHash);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT *
                FROM app_user as A
                WHERE A.email = @Email

            """
            ;
            parameters.Add("Email", email);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<AppUser>(sql, parameters);
        }

        public async Task<bool> UpdateLoginAttemptAsync(Guid userId, int failedLoginAttempts, DateTime? lockedUntil)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                UPDATE app_user
                SET failed_login_attempts = @FailedLoginAttempts, locked_until = @LockedUntil
                WHERE user_id = @UserId

            """
            ;
            parameters.Add("UserId", userId);
            parameters.Add("FailedLoginAttempts", failedLoginAttempts);
            parameters.Add("LockedUntil", lockedUntil);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateSharingEnabledAsync(Guid userId, bool sharingEnabled)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                UPDATE app_user
                SET sharing_enabled = @SharingEnabled
                WHERE user_id = @UserId

            """
            ;
            parameters.Add("UserId", userId);
            parameters.Add("SharingEnabled", sharingEnabled);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> IsSharingEnabledAsync(Guid userId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT sharing_enabled
                FROM app_user
                WHERE user_id = @UserId

            """
            ;
            parameters.Add("UserId", userId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleAsync<bool>(sql, parameters);
        }
    }
}