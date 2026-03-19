using Microsoft.AspNetCore.Authorization;
using School.API.Authorization.Requirements;
using School.Domain.Entities;
using System.Security.Claims;

namespace School.API.Authorization.Handlers;

public class StudentAccessHandler
    : AuthorizationHandler<StudentAccessRequirement, Student>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        StudentAccessRequirement requirement,
        Student resource)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userIdClaim, out var userId) &&
            resource.OwnerUserId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}