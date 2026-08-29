using Domain.Entities;

namespace Application.Interfaces.Security;
public interface IJwtProvider
{
    string GenerateToken(User user);
}
