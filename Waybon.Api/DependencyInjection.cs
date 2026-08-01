using Waybon.Application;
using Waybon.Infrastructure;
using Waybon.Application.Interfaces;
using Waybon.Api.SignalR;

namespace Waybon.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);

            services.AddScoped<ILocationBroadcaster, SignalRLocationBroadcaster>();

            return services;
        }
    }
}