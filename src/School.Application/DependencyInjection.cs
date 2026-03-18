using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Interfaces.Services;
using School.Application.Services;

namespace School.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IAuthService, AuthService>();


        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}