using Dapper;
using Waybon.Domain.Interfaces;

namespace Waybon.Infrastructure.Repositories
{
    public class SharingExceptionRepository(IDbConnectionFactory dbConnectionFactory) : ISharingExceptionRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<bool> CreateExceptionAsync(Guid sharerUserId, Guid blockedUserId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                INSERT INTO sharing_exception (sharer_user_id, blocked_user_id)
                VALUES (@SharerUserId, @BlockedUserId)

            """
            ;
            parameters.Add("SharerUserId", sharerUserId);
            parameters.Add("BlockedUserId", blockedUserId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> RemoveExceptionAsync(Guid sharerUserId, Guid blockedUserId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                DELETE FROM sharing_exception
                WHERE sharer_user_id = @SharerUserId AND blocked_user_id = @BlockedUserId

            """
            ;
            parameters.Add("SharerUserId", sharerUserId);
            parameters.Add("BlockedUserId", blockedUserId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }
    }
}