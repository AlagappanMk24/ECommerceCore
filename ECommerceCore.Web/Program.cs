using ECommerceCore.Application.DependencyInjection;
using ECommerceCore.Infrastructure.Data.Context;
using ECommerceCore.Infrastructure.Data.DbInitializer;
using ECommerceCore.Infrastructure.External.SMS;
using ECommerceCore.Infrastructure.Shared;
using ECommerceCore.Infrastructure.Shared.Security;
using ECommerceCore.Infrastructure.Utilities;
using ECommerceCore.Web.Filters;
using ECommerceCore.Web.Logger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<EcomDbContext>();

builder.Services.AddRazorPages();

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

    ConfigureJwtSettings(services, configuration);

    ConfigureJwtAuthentication(services, configuration);

    GenerateSecretKey(configuration);

    ConfigureEmailSettings(services, configuration);

    ConfigureSmsSettings(services, configuration);

    ConfigureStripeSettings(services, configuration);

    ConfigureIdentity(services);

    ConfigureApplicationCookie(services);

    ConfigureExternalLogins(builder.Services, builder.Configuration);

    ConfigureSession(services);

    RegisterApplicationServices(services);

    ConfigureStripe(configuration);
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

void ConfigureJwtSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
}

void GenerateSecretKey(IConfiguration configuration)
{
    var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

    // Check if SecretKey is null or empty and generate a new one if necessary
    if (string.IsNullOrEmpty(jwtSettings.SecretKey))
    {
        var secretKey = SecretKeyGenerator.GenerateSecretKey();
        jwtSettings ??= new JwtSettings();
        jwtSettings.SecretKey = secretKey;

        // Update appsettings.json with the new secret key
        var appSettingsFile = "appsettings.json";
        var json = System.IO.File.ReadAllText(appSettingsFile);
        dynamic jsonObj = JsonConvert.DeserializeObject(json);

        jsonObj["JwtSettings"]["SecretKey"] = secretKey;
        string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        System.IO.File.WriteAllText(appSettingsFile, output);
    }
}

/// <summary>
/// Configures jwt authentication
/// </summary>
void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration configuration)
{
    var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

    var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(key),
              ValidateIssuer = false,
              ValidateAudience = false,
              ValidateLifetime = true,
              ClockSkew = TimeSpan.Zero
          };
      });
}
/// <summary>
/// Configures Email settings from app configuration.
/// </summary>
void ConfigureEmailSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
}

/// <summary>
/// Configures SMS settings from app configuration.
/// </summary>
void ConfigureSmsSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<SMSSettings>(configuration.GetSection("SMSSettings"));
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

        // Set the cookie expiration time
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

        // Extend expiration with activity
        options.SlidingExpiration = true;

        // Prevent client-side access
        options.Cookie.HttpOnly = true;

        // Use HTTPS
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
}

/// <summary>
/// Configures external login providers, such as Facebook.
/// </summary>
void ConfigureExternalLogins(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication()
    //.AddFacebook(fbOptions =>
    //{
    //    fbOptions.AppId = configuration.GetSection("FacebookKeys:AppId").Value;
    //    fbOptions.AppSecret = configuration.GetSection("FacebookKeys:AppSecret").Value;
    //})
    .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
    {
        googleOptions.ClientId = configuration.GetSection("GoogleKeys:ClientId").Value;
        googleOptions.ClientSecret = configuration.GetSection("GoogleKeys:ClientSecret").Value;
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = configuration.GetSection("MicrosoftKeys:ClientId").Value;
        microsoftOptions.ClientSecret = configuration.GetSection("MicrosoftKeys:ClientSecret").Value;
        //microsoftOptions.CallbackPath = configuration.GetSection("MicrosoftKeys:CallbackPath").Value;
    });
}

/// <summary>
/// Configures session settings.
/// </summary>
void ConfigureSession(IServiceCollection services)
{
    services.AddDistributedMemoryCache();
    services.AddSession(Options =>
    {
        Options.IdleTimeout = TimeSpan.FromMinutes(5);
        // Prevent client-side access
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
        name: "areas",
        pattern: "{area:exists}/{controller}/{action}/{id?}"
    );

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=LandingPage}/{id?}"
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