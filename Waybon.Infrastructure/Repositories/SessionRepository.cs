using Dapper;
using Waybon.Domain.Entities;
using Waybon.Domain.Interfaces;

namespace Waybon.Infrastructure.Repositories
{
    public class SessionRepository(IDbConnectionFactory dbConnectionFactory) : ISessionRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<Session> CreateOrReplaceSessionAsync(Guid userId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                INSERT INTO session (user_id, created_at, last_activity_at)
                VALUES (@UserId, @CreatedAt, @LastActivityAt)
                ON CONFLICT (user_id)
                DO UPDATE SET
                    session_id = gen_random_uuid(),
                    created_at = @CreatedAt,
                    last_activity_at = @LastActivityAt
                RETURNING *

            """
            ;
            parameters.Add("UserId", userId);
            parameters.Add("CreatedAt", DateTime.UtcNow);
            parameters.Add("LastActivityAt", DateTime.UtcNow);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleAsync<Session>(sql, parameters);
        }

        public async Task<bool> DeleteSessionAsync(Guid sessionId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                DELETE FROM session
                WHERE session_id = @SessionId

            """
            ;
            parameters.Add("SessionId", sessionId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<Session?> GetSessionByIdAsync(Guid sessionId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT *
                FROM session as S
                WHERE S.session_id = @SessionId

            """
            ;
            parameters.Add("SessionId", sessionId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<Session>(sql, parameters);
        }
    }
}