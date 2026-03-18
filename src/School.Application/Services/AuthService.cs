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

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
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

        user.LastLoginAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAtUtc,
            UserId = user.Id.ToString(),
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles
        };
    }
}