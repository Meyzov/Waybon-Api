using System.Data;
using Waybon.Domain.Interfaces;
using Npgsql;

namespace Waybon.Infrastructure.Data
{
    public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        private readonly string _connectionString = connectionString;

        public async Task<IDbConnection> CreateConnectionAsync() // Remember to close the connection once used
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }

            return connection;
        }
    }
}