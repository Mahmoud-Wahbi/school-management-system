using School.Application.Interfaces.Common;
using School.API.Services;

namespace School.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Needed to access HttpContext outside controllers
        services.AddHttpContextAccessor();

        // Current user service (reads claims from HttpContext.User)
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}