using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Infrastructure.Data.DbInitializer;
using ECommerceCore.Infrastructure.External.Payments;
using ECommerceCore.Infrastructure.External.SMS;
using ECommerceCore.Infrastructure.Persistence;
using ECommerceCore.Infrastructure.Services;
using ECommerceCore.Infrastructure.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceCore.Application.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            // Register Unit Of Work and Generic Repository and Services
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, StripeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<ISmsSender, TwilioService>();
            services.AddScoped<IAuthStateService, AuthStateService>();
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}