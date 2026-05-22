using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProjectTaskManagement.Services;
using ProjectTaskManagement.Validators.Projects;

namespace ProjectTaskManagement;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddValidatorsFromAssemblyContaining<CreateProjectValidator>();
        return services;
    }
}
