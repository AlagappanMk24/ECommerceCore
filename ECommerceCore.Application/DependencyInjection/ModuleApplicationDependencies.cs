﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerceCore.Application.DependencyInjection
{
    public static class ModuleApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // Configuration of AutoMapper
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}