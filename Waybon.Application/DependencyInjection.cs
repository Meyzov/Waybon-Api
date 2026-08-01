using Microsoft.Extensions.DependencyInjection;
using Waybon.Application.Helpers;
using Waybon.Application.Interfaces;
using Waybon.Application.Services;

namespace Waybon.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<IConnectionManager>(sp => sp.GetRequiredService<ConnectionManager>());
            services.AddSingleton<IConnectionMetrics>(sp => sp.GetRequiredService<ConnectionManager>());

            services.AddSingleton<LocationCache>();
            services.AddSingleton<ILocationCache>(sp => sp.GetRequiredService<LocationCache>());
            services.AddSingleton<ICacheMetrics>(sp => sp.GetRequiredService<LocationCache>());

            services.AddSingleton<IMetricsCollector, MetricsCollector>();

            services.AddScoped<LocationService>();
            services.AddScoped<IGroupMembershipNotifier>(sp => sp.GetRequiredService<LocationService>());
            services.AddScoped<IUserRefreshNotifier>(sp => sp.GetRequiredService<LocationService>());

            services.AddScoped<AuthService>();
            services.AddScoped<GroupService>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}