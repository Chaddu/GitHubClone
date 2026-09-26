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

    public async Task<bool> ExistsByNameAsync(string name, int? ownerId, int? organizationId)
    {
        return await _dbSet.AnyAsync(x => x.Name == name && x.OwnerId == ownerId && x.OrganizationId == organizationId);
    }

    public async Task<Repository?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<List<Repository>> GetByOrganizationIdAsync(int organizationId)
    {
        return await _dbSet.Where(x => x.OrganizationId == organizationId).ToListAsync();
    }

    public async Task<List<Repository>> GetByOwnerIdAsync(int ownerId)
    {
        return await _dbSet.Where(r => r.OwnerId == ownerId).ToListAsync();
    }
}

    
