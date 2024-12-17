using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Infrastructure.Data.DbInitializer;
using ECommerceCore.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerceCore.Application.DependencyInjection
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            // Register Unit Of Work and Generic Repository and Services
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           
            return services;
        }
    }
}