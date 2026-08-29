using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;
public class PullRequestRepository : GenericRepository<PullRequest>, IPullRequestRepository
{
    public PullRequestRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<List<PullRequest>> GetByAuthorIdAsync(int authorId)
    {
        return await _dbSet.Where(pr => pr.AuthorId == authorId)
                       .ToListAsync();
    }

    public async Task<List<PullRequest>> GetByRepositoryIdAsync(int repositoryId)
    {
        return await _dbSet.Where(pr => pr.RepositoryId == repositoryId)
                       .ToListAsync();
    }
}
