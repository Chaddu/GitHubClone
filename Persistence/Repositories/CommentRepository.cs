using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Comment>> GetByAuthorIdAsync(int authorId)
    {
        return await _dbSet.Where(c => c.AuthorId == authorId)
                       .ToListAsync();
    }

    public async Task<List<Comment>> GetByIssueIdAsync(int issueId)
    {
        return await _dbSet.Where(c => c.IssueId == issueId)
                       .ToListAsync();
    }

    public async Task<List<Comment>> GetByPullRequestIdAsync(int pullRequestId)
    {
        return await _dbSet.Where(c => c.PullRequestId == pullRequestId)
                       .ToListAsync();
    }
}
