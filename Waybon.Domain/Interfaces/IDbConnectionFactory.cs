using System.Data;

namespace Waybon.Domain.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}