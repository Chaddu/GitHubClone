using Application.Common;
using Application.DTOs.Auth;

namespace Application.Interfaces.Services;
public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);

    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);     
}
