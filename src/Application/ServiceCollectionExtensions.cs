using System.Reflection;
using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddInfrastructure(configuration);

            services.AddTransient<IAuditoriumService, AuditoriumService>();

            return services;
        }
    }
}
