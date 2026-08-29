using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class LabelRepository : GenericRepository<Label>, ILabelRepository
{
    public LabelRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByNameAsync(int repositoryId, string name)
    {
        return await _dbSet.AnyAsync(x => x.Name == name);
    }

    public async Task<Label?> GetByNameAsync(int repositoryId, string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Label>> GetByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(x => x.RepositoryId == repositoryId).ToListAsync();
    }
}
