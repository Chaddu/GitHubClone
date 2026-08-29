using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
{
    public ActivityRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Activity>> GetByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(x => x.RepositoryId == repositoryId).ToListAsync();
    }

    public async Task<List<Activity>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
    }
}
