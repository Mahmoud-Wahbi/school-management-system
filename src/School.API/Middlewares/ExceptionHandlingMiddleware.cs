using Microsoft.Extensions.Logging;
using School.API.Common;
using School.Application.Exceptions;
using System.Net;

namespace School.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            BadRequestException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            ForbiddenException => (int)HttpStatusCode.Forbidden,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var logLevel = exception switch
        {
            ForbiddenException => LogLevel.Warning,
            NotFoundException => LogLevel.Warning,
            BadRequestException => LogLevel.Warning,
            _ => LogLevel.Error
        };

        _logger.Log(
            logLevel,
            exception,
            "Exception occurred. Path: {Path}, Method: {Method}",
            context.Request.Path,
            context.Request.Method);

        var message = exception switch
        {
            BadRequestException => exception.Message,
            NotFoundException => exception.Message,
            ForbiddenException => exception.Message,
            _ => "An unexpected error occurred."
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = ApiResponse<string>.FailureResponse(message);
        return context.Response.WriteAsJsonAsync(response);
    }
}