using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByEmailAsync(string email);

    Task<bool> ExistsByUsernameAsync(string username);

    Task<bool> ExistsByEmailAsync(string email);
}
