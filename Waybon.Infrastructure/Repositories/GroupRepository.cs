using Dapper;
using Waybon.Domain.Entities;
using Waybon.Domain.Interfaces;

namespace Waybon.Infrastructure.Repositories
{
    public class GroupRepository(IDbConnectionFactory dbConnectionFactory) : IGroupRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task CreateGroupAsync(UserGroup userGroup)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var parameters = new DynamicParameters();
                var sql =
                """

                    INSERT INTO user_group (owner_user_id, name, join_code, join_code_expires_at, created_at)
                    VALUES (@OwnerUserId, @Name, @JoinCode, @JoinCodeExpiresAt, @CreatedAt)
                    RETURNING *

                """
                ;
                parameters.Add("OwnerUserId", userGroup.OwnerUserId);
                parameters.Add("Name", userGroup.Name);
                parameters.Add("JoinCode", userGroup.JoinCode);
                parameters.Add("JoinCodeExpiresAt", DateTime.UtcNow.AddMinutes(30));
                parameters.Add("CreatedAt", DateTime.UtcNow);

                var result = await connection.QuerySingleAsync<UserGroup>(sql, parameters, transaction);
                parameters = new DynamicParameters();
                sql =
                """

                    INSERT INTO group_member (group_id, user_id, joined_at)
                    VALUES (@GroupId, @UserId, @JoinedAt)

                """
                ;
                parameters.Add("GroupId", result.GroupId);
                parameters.Add("UserId", userGroup.OwnerUserId);
                parameters.Add("JoinedAt", DateTime.UtcNow);

                var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction);
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Failed to add owner to group members.");
                }
                
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<UserGroup?> GetGroupByIdAsync(int groupId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT *
                FROM user_group AS U
                WHERE U.group_id = @GroupId

            """
            ;
            parameters.Add("GroupId", groupId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<UserGroup>(sql, parameters);
        }

        public async Task<bool> DeleteGroupAsync(int groupId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                DELETE FROM user_group
                WHERE group_id = @GroupId

            """
            ;
            parameters.Add("GroupId", groupId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<UserGroup?> GetGroupByJoinCodeAsync(string joinCode)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT *
                FROM user_group as U
                WHERE U.join_code = @JoinCode

            """
            ;
            parameters.Add("JoinCode", joinCode);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<UserGroup>(sql, parameters);
        }

        public async Task<UserGroup?> RegenerateJoinCodeAsync(int groupId, string newJoinCode)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                UPDATE user_group
                SET join_code = @JoinCode, join_code_expires_at = @JoinCodeExpiresAt
                WHERE group_id = @GroupId
                RETURNING *

            """
            ;
            parameters.Add("GroupId", groupId);
            parameters.Add("JoinCode", newJoinCode);
            parameters.Add("JoinCodeExpiresAt", DateTime.UtcNow.AddMinutes(30));

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<UserGroup>(sql, parameters);
        }

        public async Task<IEnumerable<UserGroup>> GetGroupsByUserIdAsync(Guid userId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT
                    U.group_id,
                    U.owner_user_id,
                    A.username,
                    U.name,
                    CASE WHEN @UserId = U.owner_user_id
                        THEN U.join_code ELSE NULL END AS join_code,
                    CASE WHEN @UserId = U.owner_user_id
                        THEN U.join_code_expires_at ELSE NULL END AS join_code_expires_at,
                    U.created_at
                FROM user_group as U
                JOIN group_member as G ON U.group_id = G.group_id
                JOIN app_user as A ON U.owner_user_id = A.user_id
                WHERE G.user_id = @UserId

            """
            ;
            parameters.Add("UserId", userId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<UserGroup>(sql, parameters);
        }
    }
}