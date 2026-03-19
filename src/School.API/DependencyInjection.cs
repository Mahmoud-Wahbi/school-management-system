using Microsoft.AspNetCore.Authorization;
using School.API.Authorization.Handlers;
using School.API.Services;
using School.Application.Interfaces.Common;

namespace School.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Needed to access HttpContext outside controllers
        services.AddHttpContextAccessor();

        // Current user service (reads claims from HttpContext.User)
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IAuthorizationHandler, StudentAccessHandler>();

        services.AddScoped<IStudentAuthorizationService, StudentAuthorizationService>();


        return services;
    }
}