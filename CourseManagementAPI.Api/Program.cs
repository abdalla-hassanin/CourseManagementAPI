using System.Security.Claims;
using System.Text;
using CourseManagementAPI.Core;
using CourseManagementAPI.Core.Base.MiddleWare;
using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Data.Options;
using CourseManagementAPI.Infrastructure;
using CourseManagementAPI.Infrastructure.Context;
using CourseManagementAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging

try 
{
    ConfigureLogging(builder);
}
catch (Exception ex)
{
    Console.WriteLine($"Logging configuration failed: {ex.Message}");
}

try
{
    Log.Information("Application starting in {Environment} environment", builder.Environment.EnvironmentName);
    // Add services to the DI container
    try
    {
        ConfigureServices(builder.Services, builder.Configuration);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Service configuration failed: {ex.Message}");
    }
    var app = builder.Build();

    // Configure middleware pipeline
    ConfigureMiddleware(app);

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

return;

void ConfigureLogging(WebApplicationBuilder applicationBuilder)
{
    applicationBuilder.Logging.ClearProviders();
    var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(applicationBuilder.Configuration);

    if (applicationBuilder.Environment.IsDevelopment())
    {
        Log.Information("Development logging configured to seq.");
    }
    else
    {
        //TODO: add Logger to server
        Log.Information("Production logging configured to ???.");
    }

    applicationBuilder.Host.UseSerilog(loggerConfiguration.CreateLogger());
}


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddOptions<SecretOptions>()
        .Bind(configuration.GetSection(SecretOptions.SectionName))
        .ValidateDataAnnotations();

    ConfigureDatabase(services, configuration);
    ConfigureAuthenticationAndAuthorization(services, configuration);
    ConfigureIdentity(services);
    ConfigureSwagger(services);

    // Add Controllers and Middleware
    services.AddControllers()
        .AddControllersAsServices();

    // Enable CORS
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    // Application-specific service registration
    RegisterApplicationServices(services);
}

void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    var secrets = configuration.GetSection(SecretOptions.SectionName).Get<SecretOptions>();

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(secrets.ConnectionStrings.DefaultConnection,
            sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null)
                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
}


void ConfigureIdentity(IServiceCollection services)
{
    services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
}


void ConfigureAuthenticationAndAuthorization(IServiceCollection services, IConfiguration configuration)
{
    var secrets = configuration.GetSection(SecretOptions.SectionName).Get<SecretOptions>();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = secrets!.JwtSecrets.Issuer,
                ValidAudience = secrets.JwtSecrets.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets.JwtSecrets.Key)),
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });

    services.AddAuthorization();
}


void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Course Management API", Version = "v1" });

        // Enable Annotations (for [FromBody], [FromQuery], etc.)
        opt.EnableAnnotations();
        // Add Swagger examples
        opt.ExampleFilters();
        // Add Authorization header
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter 'Bearer' [space] and then your token.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    });

    // Register Swagger example providers
    services.AddSwaggerExamplesFromAssemblyOf<Program>();
}


void RegisterApplicationServices(IServiceCollection services)
{
    services
        .AddServiceDependencies()
        .AddCoreDependencies()
        .AddInfrastructureDependencies();
}

void ConfigureMiddleware(WebApplication app)
{
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();

    // Configure Swagger
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course Management API v1");
            c.RoutePrefix = string.Empty;
            c.DisplayRequestDuration();
        }
    );

    // Custom error-handling middleware
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
}