using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Auth;
using School.Application.Interfaces.Services;
using School.API.Common;
using School.Application.Interfaces.Common;

namespace School.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly ICurrentUserService _currentUser;

    public AuthController(
        IAuthService authService,
        ICurrentUserService currentUser)
    {
        _authService = authService;
        _currentUser = currentUser;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(
            result,
            "Login successful."));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> RefreshToken(
    [FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(
            result,
            "Token refreshed successfully."));
    }


    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            UserId = _currentUser.UserId,
            Email = _currentUser.Email,
            FullName = _currentUser.FullName,
            Roles = _currentUser.Roles,
            IsAuthenticated = _currentUser.IsAuthenticated
        });
    }
}