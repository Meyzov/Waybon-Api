using Dapper;
using Waybon.Domain.Entities;
using Waybon.Domain.Interfaces;

namespace Waybon.Infrastructure.Repositories
{
    public class RoleRepository(IDbConnectionFactory dbConnectionFactory) : IRoleRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT *
                FROM role as R
                WHERE R.name = @Name

            """
            ;
            parameters.Add("Name", name);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<Role>(sql, parameters);
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            var parameters = new DynamicParameters();
            var sql =
            """

                SELECT *
                FROM role as R
                WHERE R.role_id = @RoleId

            """
            ;
            parameters.Add("RoleId", roleId);

            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<Role>(sql, parameters);
        }
    }
}