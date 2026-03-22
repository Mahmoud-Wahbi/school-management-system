namespace School.Application.Interfaces.Services;

public interface IRefreshTokenGenerator
{
    string GenerateToken();
}