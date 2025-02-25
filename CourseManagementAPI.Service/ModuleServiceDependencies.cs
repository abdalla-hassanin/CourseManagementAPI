using CourseManagementAPI.Service.IService;
using CourseManagementAPI.Service.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CourseManagementAPI.Service;

public static class ModuleServiceDependencies
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        // Register Entity Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }
}