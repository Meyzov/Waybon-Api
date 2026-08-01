using Waybon.Domain.Entities;

namespace Waybon.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByNameAsync(string name);
        Task<Role?> GetRoleByIdAsync(int roleId);
    }
}