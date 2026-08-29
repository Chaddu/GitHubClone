using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(x => x.Name == name);
    }

    public async Task<Organization?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Organization>> GetByOwnerIdAsync(int ownerId)
    {
        return await _dbSet.Where(x => x.OwnerId == ownerId).ToListAsync();
    }
}
