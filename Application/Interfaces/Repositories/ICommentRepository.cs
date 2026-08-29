using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<List<Comment>> GetByIssueIdAsync(int issueId);

    Task<List<Comment>> GetByPullRequestIdAsync(int pullRequestId);

    Task<List<Comment>> GetByAuthorIdAsync(int authorId);
}
