using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Review?> GetByPullRequestAndReviewerAsync(int pullRequestId, int reviewerId)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.PullRequestId == pullRequestId && r.ReviewerId == reviewerId);
    }

    public async Task<List<Review>> GetByPullRequestIdAsync(int pullRequestId)
    {
        return await _dbSet.Where(r => r.PullRequestId == pullRequestId).ToListAsync();
    }

    public async Task<List<Review>> GetByReviewerIdAsync(int reviewerId)
    {
        return await _dbSet.Where(r => r.ReviewerId == reviewerId).ToListAsync();
    }
}
