using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Waybon.Domain.Interfaces;
using Waybon.Infrastructure.Data;
using Waybon.Infrastructure.Repositories;

namespace Waybon.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnectionFactory>
            (
                sp => new DbConnectionFactory
                (
                    configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.")
                )
            );

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ISharingExceptionRepository, SharingExceptionRepository>();

            return services;
        }
    }
}