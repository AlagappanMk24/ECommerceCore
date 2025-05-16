using ECommerceCore.Application.DependencyInjection;
using ECommerceCore.Infrastructure.Data.Context;
using ECommerceCore.Infrastructure.Data.DbInitializer;
using ECommerceCore.Infrastructure.Shared;
using ECommerceCore.Infrastructure.Shared.Security;
using ECommerceCore.Infrastructure.Utilities;
using ECommerceCore.Web.Filters;
using ECommerceCore.Web.Logger;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Stripe;
using System.Text;
using Newtonsoft.Json.Converters;
using ECommerceCore.Application.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;
using ECommerceCore.Application.Authorization.Requirements;
using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Infrastructure.Services.Authorization;
using ECommerceCore.Domain.Entities.Identity;
using ECommerceCore.Infrastructure.Services.Background;

var builder = WebApplication.CreateBuilder(args);

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

    services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

    services.AddControllersWithViews();

    services.AddHttpContextAccessor();

    services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
        //options.LowercaseQueryStrings = true; // optional
    });

    services.AddRazorPages();

    services.Configure<UserCleanupOptions>(options =>
    {
        options.RetentionDays = configuration.GetValue<int>("UserCleanup:RetentionDays", 30);
    });

    services.AddHostedService<UserCleanupService>();

    ConfigureDatabase(services, configuration);

    ConfigureJwtSettings(services, configuration);

    ConfigureAuthentication(services, configuration);

    ConfigureAuthorization(services);

    GenerateSecretKey(configuration);

    ConfigureAdminSettings(services, configuration);

    ConfigureEmailSettings(services, configuration);

    ConfigureSmsSettings(services, configuration);

    ConfigureStripeSettings(services, configuration);

    ConfigureIdentity(services);

    ConfigureApplicationCookie(services);

    ConfigureExternalLogins(builder.Services, builder.Configuration);

    ConfigureSession(services);

    services.AddAuthorization();

    RegisterApplicationServices(services);

    ConfigureStripe(configuration);
}
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
void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
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

void ConfigureAuthorization(IServiceCollection services)
{
    // Register the permission handler
    services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

    //services.AddScoped<IPermissionService, PermissionService>();

    // Configure authorization policies
    services.AddAuthorization(options =>
    {
        // Register policies for all defined permissions
        foreach (var permissionField in typeof(AuthorizationConstants.Permissions)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
        {
            if (permissionField.FieldType == typeof(string))
            {
                var permission = (string)permissionField.GetValue(null);
                options.AddPolicy(permission, policy =>
                    policy.Requirements.Add(new PermissionRequirement(permission)));
            }
        }

        // Grouped policies for common use cases
        options.AddPolicy("ProductAccess", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.IsInRole(AppConstants.Role_Vendor) ||
                context.User.IsInRole(AppConstants.Role_Supplier) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    (c.Value == AuthorizationConstants.Permissions.Product_View ||
                     c.Value == AuthorizationConstants.Permissions.Product_Manage))));

        options.AddPolicy("ProductManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.Product_Manage)));

        options.AddPolicy("OrderAccess", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.IsInRole(AppConstants.Role_CustomerSupport) ||
                context.User.IsInRole(AppConstants.Role_DeliveryAgent) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    (c.Value == AuthorizationConstants.Permissions.Order_View ||
                     c.Value == AuthorizationConstants.Permissions.Order_Manage))));

        options.AddPolicy("OrderManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.Order_Manage)));

        options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole(AppConstants.Role_Admin, AppConstants.Role_Admin_Super));

        options.AddPolicy("UserManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.User_Manage)));

        // Additional grouped policies for other entities
        options.AddPolicy("CategoryManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.Category_Manage)));

        options.AddPolicy("InvoiceManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.Invoice_Manage)));

        options.AddPolicy("CustomerManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.IsInRole(AppConstants.Role_CustomerSupport) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.Customer_Manage)));

        options.AddPolicy("CompanyManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.Company_Manage)));

        options.AddPolicy("BrandManagement", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(AppConstants.Role_Admin) ||
                context.User.IsInRole(AppConstants.Role_Admin_Super) ||
                context.User.IsInRole(AppConstants.Role_Manager) ||
                context.User.HasClaim(c => c.Type == "Permission" &&
                    c.Value == AuthorizationConstants.Permissions.Brand_Manage)));
    });
}

void ConfigureAdminSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<AdminSettings>(configuration.GetSection("AdminSettings"));
}
void ConfigureEmailSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
}
void ConfigureSmsSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<SMSSettings>(configuration.GetSection("SMSSettings"));
}
void ConfigureStripeSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
}
void ConfigureIdentity(IServiceCollection services)
{
    services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<EcomDbContext>()
            .AddDefaultTokenProviders();
}
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
void RegisterApplicationServices(IServiceCollection services)
{
    services.AddApplicationDependencies()
            .AddInfrastructureDependencies();
}
void ConfigureStripe(IConfiguration configuration)
{
    StripeConfiguration.ApiKey = configuration.GetSection("Stripe:SecretKey").Get<string>();
}
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
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        //dbInitializer.Initialize();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            // Create a cancellation token with a timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            dbInitializer.Initialize(cts.Token).GetAwaiter().GetResult();
            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Database seeding was canceled: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database seeding: {Message}", ex.Message);
            throw;
        }
    }
}
void ConfigureCustomLogging(WebApplication app)
{
    string formattedDate = DateTime.Now.ToString("MM-dd-yyyy");
    string baseLogPath = builder.Configuration.GetValue<string>("Logging:LogFilePath");
    string logFilePath = Path.Combine(baseLogPath, $"log-{formattedDate}.txt");

    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
    loggerFactory.AddProvider(new CustomFileLoggerProvider(logFilePath, httpContextAccessor));
}
