using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IReviewRepository : IGenericRepository<Review>
{
    Task<List<Review>> GetByPullRequestIdAsync(int pullRequestId);

    Task<List<Review>> GetByReviewerIdAsync(int reviewerId);

    Task<Review?> GetByPullRequestAndReviewerAsync(int pullRequestId,int reviewerId);
}
