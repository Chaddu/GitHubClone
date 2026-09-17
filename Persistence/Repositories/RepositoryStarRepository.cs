using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class RepositoryStarRepository : GenericRepository<RepositoryStar>, IRepositoryStarRepository
{
    public RepositoryStarRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<RepositoryStar?> GetByRepositoryAndUserAsync(int repositoryId, int userId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.RepositoryId == repositoryId && x.UserId == userId);
    }

    public async Task<List<RepositoryStar>> GetByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(x => x.RepositoryId == repositoryId).ToListAsync();
    }

    public async Task<List<RepositoryStar>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
    }
}
