using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }
}
