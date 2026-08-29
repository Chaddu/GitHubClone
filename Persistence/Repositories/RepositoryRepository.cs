using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;
public class RepositoryRepository : GenericRepository<Repository>, IRepositoryRepository
{
    public RepositoryRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(r => r.Name == name);
    }

    public async Task<Repository?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<List<Repository>> GetByOwnerIdAsync(int ownerId)
    {
        return await _dbSet.Where(r => r.OwnerId == ownerId).ToListAsync();
    }
}

    
