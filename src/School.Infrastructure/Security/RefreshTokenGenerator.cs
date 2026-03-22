using System.Security.Cryptography;
using School.Application.Interfaces.Services;

namespace School.Infrastructure.Security;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string GenerateToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}