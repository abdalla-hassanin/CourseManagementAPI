using CourseManagementAPI.Infrastructure.Base;
using Microsoft.Extensions.DependencyInjection;

namespace CourseManagementAPI.Infrastructure;

public static class ModuleInfrastructureDependencies
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        // Register UnitOfWork and GenericRepository
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}