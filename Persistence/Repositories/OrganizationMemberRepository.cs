using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrganizationMemberRepository : GenericRepository<OrganizationMember>, IOrganizationMemberRepository
{

    public OrganizationMemberRepository(AppDbContext context) : base(context)
    {

    }
    public async Task<List<OrganizationMember>> GetByOrganizationIdAsync(int organizationId)
    {
        return await _dbSet.Where(m => m.OrganizationId == organizationId).ToListAsync();
    }

    public async Task<List<OrganizationMember>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Where(m => m.UserId == userId).ToListAsync();
    }

    public async Task<OrganizationMember?> GetMembershipAsync(int organizationId, int userId)
    {
        return await _dbSet.FirstOrDefaultAsync(m => m.OrganizationId == organizationId && m.UserId == userId);
    }

    public async Task<bool> IsMemberAsync(int organizationId, int userId)
    {
        return await _dbSet.AnyAsync(m => m.OrganizationId == organizationId && m.UserId == userId);
    }
}

