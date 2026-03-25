using Microsoft.AspNetCore.RateLimiting;
using School.API.Common;
using System.Threading.RateLimiting;

namespace School.API.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authLimit = configuration.GetValue<int>("RateLimiting:Auth:PermitLimit");
        var authWindow = configuration.GetValue<int>("RateLimiting:Auth:WindowInMinutes");
        var defaultLimit = configuration.GetValue<int>("RateLimiting:Default:PermitLimit");
        var defaultWindow = configuration.GetValue<int>("RateLimiting:Default:WindowInMinutes");

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();

                logger.LogWarning(
                    "Rate limit exceeded. Path: {Path}, IP: {IP}",
                    context.HttpContext.Request.Path,
                    context.HttpContext.Connection.RemoteIpAddress);

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                var response = ApiResponse<string>.FailureResponse(
                    "Too many requests. Please try again later.");

                await context.HttpContext.Response.WriteAsJsonAsync(
                    response, cancellationToken);
            };

            options.AddPolicy("auth", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.IsAuthenticated == true
                        ? httpContext.User.Identity!.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous"
                        : httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = authLimit,
                        Window = TimeSpan.FromMinutes(authWindow),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));

            options.AddPolicy("default", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.IsAuthenticated == true
                        ? httpContext.User.Identity!.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous"
                        : httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = defaultLimit,
                        Window = TimeSpan.FromMinutes(defaultWindow),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));
        });

        return services;
    }
}