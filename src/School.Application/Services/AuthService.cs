using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Auth;
using School.Application.Exceptions;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace School.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ILogger<AuthService> _logger;
    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IRefreshTokenGenerator refreshTokenGenerator,
        ILogger<AuthService> logger
        )
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenGenerator = refreshTokenGenerator;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork
            .Users
            .GetQueryable()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null)
        {
            _logger.LogWarning("Login failed. User not found for email: {Email}", request.Email);
            throw new BadRequestException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed. Inactive account for email: {Email}", request.Email);
            throw new BadRequestException("This user account is inactive.");
        }

        var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed. Invalid password for email: {Email}", request.Email);
            throw new BadRequestException("Invalid email or password.");
        }

        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .ToList();

        var (accessToken, expiresAtUtc) =
            await _tokenService.GenerateAccessTokenAsync(user, roles);

        var refreshTokenValue = _refreshTokenGenerator.GenerateToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = false
        };
        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();


        user.LastLoginAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(
        "Login succeeded for user {UserId} with email {Email}",
        user.Id,
        user.Email);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAtUtc,
            RefreshToken = refreshTokenValue,
            RefreshTokenExpiresAtUtc = refreshToken.ExpiresAt,
            UserId = user.Id.ToString(),
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var refreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken);

        if (refreshToken is null)
        {
            _logger.LogWarning("Refresh token failed. Token not found.");
            throw new BadRequestException("Invalid refresh token.");
        }

        if (!refreshToken.IsActive)
        {
            _logger.LogWarning(
                                "Refresh token rejected for user {UserId}. Token inactive or already used.",
                                refreshToken.UserId);

            throw new BadRequestException("Refresh token is no longer valid.");
        }

        var user = refreshToken.User;

        if (!user.IsActive)
        {
            _logger.LogWarning(
                                "Refresh token failed. Inactive account for user {UserId}",
                                user.Id);

            throw new BadRequestException("This user account is inactive.");
        }

        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .ToList();

        var (accessToken, expiresAtUtc) =
            await _tokenService.GenerateAccessTokenAsync(user, roles);

        refreshToken.IsUsed = true;
        refreshToken.RevokedAt = DateTime.UtcNow;

        var newRefreshTokenValue = _refreshTokenGenerator.GenerateToken();

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = false
        };

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(
                            "Refresh token succeeded for user {UserId}. Token rotation completed.",
                            user.Id);
        return new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAtUtc,
            RefreshToken = newRefreshTokenValue,
            RefreshTokenExpiresAtUtc = newRefreshToken.ExpiresAt,
            UserId = user.Id.ToString(),
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles
        };
    }

}