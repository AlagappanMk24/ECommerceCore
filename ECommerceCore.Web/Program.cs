using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.DependencyInjection;
using ECommerceCore.Infrastructure.Data.Context;
using ECommerceCore.Infrastructure.Data.DbInitializer;
using ECommerceCore.Infrastructure.Repositories;
using ECommerceCore.Infrastructure.Utilities;
using ECommerceCore.Web.Filters;
using ECommerceCore.Web.Logger;
using ECommerceCore.Web.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<EcomDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IEmailSender, EmailSender>();

/// <summary>
/// Configures all required services for the application.
/// </summary>
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers(options =>
    {
        // Custom filter for model validation
        options.Filters.Add<ValidateModelFilter>();
    });

    services.AddControllersWithViews();

    services.AddHttpContextAccessor();

    ConfigureDatabase(services, configuration);

    ConfigureStripeSettings(services, configuration);

    ConfigureIdentity(services);

    ConfigureApplicationCookie(services);

    ConfigureExternalLogins(builder.Services, builder.Configuration);

    ConfigureSession(services);

    RegisterApplicationServices(services);

    ConfigureStripe(configuration);
}

/// <summary>
/// Configures Stripe settings from app configuration.
/// </summary>
void ConfigureStripeSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
}

/// <summary>
/// Configures Identity services for authentication and token providers.
/// </summary>
void ConfigureIdentity(IServiceCollection services)
{
    services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<EcomDbContext>()
            .AddDefaultTokenProviders();
}

/// <summary>
/// Configures Application Cookie settings to handle login/logout and access denied paths.
/// </summary>
void ConfigureApplicationCookie(IServiceCollection services)
{
    services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    });
}

/// <summary>
/// Configures external login providers, such as Facebook.
/// </summary>
void ConfigureExternalLogins(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication().AddFacebook(options =>
    {
        options.AppId = "";
        options.AppSecret = "";
    });
}

/// <summary>
/// Configures the database context using SQL Server.
/// </summary>
void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("EcomDbConnection");

    // Add DbContext with SQL Server
    services.AddDbContext<EcomDbContext>(options =>
        options.UseSqlServer(connectionString));
}

/// <summary>
/// Configures session settings.
/// </summary>
void ConfigureSession(IServiceCollection services)
{
    services.AddDistributedMemoryCache();
    services.AddSession(Options => {
        Options.IdleTimeout = TimeSpan.FromMinutes(100);
        Options.Cookie.HttpOnly = true;
        Options.Cookie.IsEssential = true;
    });
}

/// <summary>
/// Registers application and infrastructure dependencies.
/// </summary>
void RegisterApplicationServices(IServiceCollection services)
{
    services.AddApplicationDependencies()
            .AddInfrastructureDependencies();
}

/// <summary>
/// Configures Stripe API key.
/// </summary>
void ConfigureStripe(IConfiguration configuration)
{
    StripeConfiguration.ApiKey = configuration.GetSection("Stripe:SecretKey").Get<string>();
}

/// <summary>
/// Configures the middleware pipeline for the application.
/// </summary>
void ConfigureMiddleware(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSession();

    app.MapRazorPages();

    app.MapControllerRoute(
        name: "default",
        pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
    );

    SeedDatabase();
    // Configure Logging
    ConfigureCustomLogging(app);
}

/// <summary>
/// Seeds the database with initial roles and admin user.
/// </summary>
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}

/// <summary>
/// Configures custom file logging using a dynamic log file path.
/// </summary>
void ConfigureCustomLogging(WebApplication app)
{
    string formattedDate = DateTime.Now.ToString("MM-dd-yyyy");
    string baseLogPath = builder.Configuration.GetValue<string>("Logging:LogFilePath");
    string logFilePath = Path.Combine(baseLogPath, $"log-{formattedDate}.txt");

    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
    loggerFactory.AddProvider(new CustomFileLoggerProvider(logFilePath, httpContextAccessor));
}