using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Auth;
using School.Application.Exceptions;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;

namespace School.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IRefreshTokenGenerator refreshTokenGenerator
        )
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenGenerator = refreshTokenGenerator;
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
            throw new BadRequestException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new BadRequestException("This user account is inactive.");
        }

        var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
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
            throw new BadRequestException("Invalid refresh token.");
        }

        if (!refreshToken.IsActive)
        {
            throw new BadRequestException("Refresh token is no longer valid.");
        }

        var user = refreshToken.User;

        if (!user.IsActive)
        {
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

        _unitOfWork.RefreshTokens.Update(refreshToken);
        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

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