using Dapper;
using Waybon.Domain.Entities;
using Waybon.Domain.Interfaces;

namespace Waybon.Infrastructure.Repositories
{
    public class GroupMemberRepository(IDbConnectionFactory dbConnectionFactory) : IGroupMemberRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<bool> AddMemberAsync(int groupId, Guid userId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                INSERT INTO group_member (group_id, user_id, joined_at)
                VALUES (@GroupId, @UserId, @JoinedAt)

            """
            ;
            parameters.Add("GroupId", groupId);
            parameters.Add("UserId", userId);
            parameters.Add("JoinedAt", DateTime.UtcNow);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> IsMemberAsync(int groupId, Guid userId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT EXISTS
                (
                    SELECT 1
                    FROM group_member AS G
                    WHERE G.group_id = @GroupId AND G.user_id = @UserId
                )

            """
            ;
            parameters.Add("GroupId", groupId);
            parameters.Add("UserId", userId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleAsync<bool>(sql, parameters);
        }

        public async Task<IEnumerable<GroupMemberDetail>> GetMembersByGroupIdAsync(int groupId, Guid requestingUserId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT
                    U.user_id,
                    U.username,
                    U.sharing_enabled,
                    S.last_activity_at,
                    B.blocked_by_me,
                    B.blocking_me
                FROM group_member AS G
                    JOIN app_user AS U ON U.user_id = G.user_id
                    LEFT JOIN session AS S ON S.user_id = U.user_id
                    CROSS JOIN LATERAL
                    (
                        SELECT
                            EXISTS
                            (
                                SELECT 1
                                FROM sharing_exception E1
                                WHERE E1.sharer_user_id = @RequestingUserId
                                AND E1.blocked_user_id = U.user_id
                            ) AS blocked_by_me,
                            EXISTS
                            (
                                SELECT 1
                                FROM sharing_exception E2
                                WHERE E2.sharer_user_id = U.user_id
                                AND E2.blocked_user_id = @RequestingUserId
                            ) AS blocking_me
                    ) AS B
                WHERE G.group_id = @GroupId

            """
            ;
            parameters.Add("GroupId", groupId);
            parameters.Add("RequestingUserId", requestingUserId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<GroupMemberDetail>(sql, parameters);
        }

        public async Task<bool> RemoveMemberAsync(int groupId, Guid userId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """ 

                DELETE FROM group_member
                WHERE group_id = @GroupId AND user_id = @UserId

            """
            ;
            parameters.Add("GroupId", groupId);
            parameters.Add("UserId", userId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Guid>> GetRecipientsForUserAsync(Guid userId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """
            
                SELECT DISTINCT G2.user_id
                FROM group_member AS G1
                    JOIN group_member AS G2 ON G2.group_id = G1.group_id AND G2.user_id != G1.user_id
                WHERE G1.user_id = @UserId
                    AND NOT EXISTS
                    (
                        SELECT 1
                        FROM sharing_exception AS S
                        WHERE S.sharer_user_id = G1.user_id
                            AND S.blocked_user_id = G2.user_id
                    )
            
            """
            ;
            parameters.Add("UserId", userId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Guid>(sql, parameters);
        }

        public async Task<IEnumerable<Guid>> GetMembersUserIdsAsync(int groupId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT user_id
                FROM group_member
                WHERE group_id = @GroupId

            """
            ;
            parameters.Add("GroupId", groupId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Guid>(sql, parameters);
        }
    }
}