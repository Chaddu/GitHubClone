using Application.Common;
using Application.DTOs.Auth;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using BCrypt.Net;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class AuthService : IAuthService
{

    private readonly IUserService _userService;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(IUserService userService, IJwtProvider jwtProvider)
    {
        _userService = userService;
        _jwtProvider = jwtProvider;
    }
    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var userResult = await _userService.GetByUsernameAsync(request.Username);

        if(!userResult.IsSuccess)
            return Result<AuthResponse>.Failure(
            "Invalid username or password.");


        var user = userResult.Value!;

        var passwordValid =
       BCrypt.Net.BCrypt.Verify(
           request.Password,
           user.PasswordHash);

        if (!passwordValid)
            return Result<AuthResponse>.Failure(
                "Invalid username or password.");

        var token = _jwtProvider.GenerateToken(user);

        return Result<AuthResponse>.Success(
            new AuthResponse
            {
                Token = token
            });
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var userExists = await _userService.ExistsByUsernameAsync(request.Username);
        if (!userExists.IsSuccess)
            return Result<AuthResponse>.Failure(userExists.Error!);

        if (userExists.Value)
            return Result<AuthResponse>.Failure("Username is already taken.");

        var emailExists = await _userService.ExistsByEmailAsync(request.Email);

        if (!emailExists.IsSuccess)
            return Result<AuthResponse>.Failure(emailExists.Error!);

        if (emailExists.Value)
            return Result<AuthResponse>.Failure("Email is already taken.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.User
        };

        var createResult = await _userService.AddAsync(user);
        if(!createResult.IsSuccess)
            return Result<AuthResponse>.Failure(createResult.Error!);

        var token = _jwtProvider.GenerateToken(user);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = token
        });
    }
}
