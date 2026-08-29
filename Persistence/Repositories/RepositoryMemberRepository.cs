using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class RepositoryMemberRepository : GenericRepository<RepositoryMember>, IRepositoryMemberRepository
{
    public RepositoryMemberRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<RepositoryMember>> GetByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(x => x.RepositoryId == repositoryId).ToListAsync();
    }

    public async Task<List<RepositoryMember>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<RepositoryMember?> GetMembershipAsync(int repositoryId, int userId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.RepositoryId == repositoryId && x.UserId == userId);
    }

    public async Task<bool> IsMemberAsync(int repositoryId, int userId)
    {
        return await _dbSet.AnyAsync(x => x.RepositoryId == repositoryId && x.UserId == userId);
    }
}
