using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuditoriumRepository, AuditoriumRepository>();
            services.AddScoped<IAuditoriumReserveRepository, AuditoriumReserveRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContextPool<TestTaskDbContext>(x => x.UseNpgsql(configuration.GetConnectionString("TestTaskConnection")));

            return services;
        }
    }
}
