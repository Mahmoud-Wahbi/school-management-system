using School.Domain.Entities;

namespace School.Application.Interfaces.Services;

public interface ITokenService
{
    Task<(string Token, DateTime ExpiresAtUtc)> GenerateAccessTokenAsync(User user, List<string> roles);
}