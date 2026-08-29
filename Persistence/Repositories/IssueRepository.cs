using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class IssueRepository : GenericRepository<Issue>, IIssueRepository
{
    public IssueRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<List<Issue>> GetByAuthorIdAsync(int authorId)
    {
        return await _dbSet.Where(i => i.CreatorId == authorId)
                       .ToListAsync();
    }

    public async Task<List<Issue>> GetByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(i => i.RepositoryId == repositoryId)
                       .ToListAsync();
    }

    public async Task<List<Issue>> GetOpenIssuesByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(i => i.RepositoryId == repositoryId && i.Status == IssueStatus.Open)
                       .ToListAsync();
    }
}
