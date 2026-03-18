namespace School.Application.Interfaces.Common;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    string? FullName { get; }

    bool IsAuthenticated { get; }

    List<string> Roles { get; }
}