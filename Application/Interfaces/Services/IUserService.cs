using Application.Common;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IUserService 
{
    Task<Result<User>> GetByIdAsync(int id);

    Task<Result<User>> GetByUsernameAsync(string username);

    Task<Result<User>> GetByEmailAsync(string email);

    Task<Result<bool>> ExistsByUsernameAsync(string username);

    Task<Result<bool>> ExistsByEmailAsync(string email);

    Task<Result> AddAsync(User user);

    Task<Result> UpdateAsync(User user);

    Task<Result> DeleteAsync(int id);
}
