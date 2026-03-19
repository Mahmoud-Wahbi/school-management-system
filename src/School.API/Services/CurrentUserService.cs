using System.Security.Claims;
using School.Application.Interfaces.Common;

namespace School.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(id, out var userId) ? userId : null;
        }
    }

    public string? Email =>
        User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? FullName =>
        User?.FindFirst(ClaimTypes.Name)?.Value;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) =>
    Roles.Contains(role, StringComparer.OrdinalIgnoreCase);

    public List<string> Roles =>
        User?.FindAll(ClaimTypes.Role)
            .Select(r => r.Value)
            .ToList()
        ?? new List<string>();
}