using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;
public class BranchRepository : GenericRepository<Branch>, IBranchRepository
{
    public BranchRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<bool> ExistsByNameAsync(int repositoryId, string name)
    {
        return await _dbSet.AnyAsync(b => b.RepositoryId == repositoryId && b.Name == name);
    }

    public async Task<Branch?> GetByNameAsync(int repositoryId, string name)
    {
        return await _dbSet.FirstOrDefaultAsync(b => b.RepositoryId == repositoryId && b.Name == name);
    }

    public async Task<List<Branch>> GetByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(b => b.RepositoryId == repositoryId).ToListAsync();
    }
}
