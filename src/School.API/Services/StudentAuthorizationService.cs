using Microsoft.AspNetCore.Authorization;
using School.Application.Exceptions;
using School.Application.Interfaces.Common;
using School.Domain.Entities;

namespace School.API.Services;

public class StudentAuthorizationService : IStudentAuthorizationService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StudentAuthorizationService(
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task EnsureCanAccessAsync(Student student)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            throw new ForbiddenException("You are not allowed to access this student.");
        }

        var result = await _authorizationService.AuthorizeAsync(
            user,
            student,
            "StudentAccess");

        if (!result.Succeeded)
        {
            throw new ForbiddenException("You are not allowed to access this student.");
        }
    }
}