using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IPullRequestRepository : IGenericRepository<PullRequest>
{
    Task<List<PullRequest>> GetByRepositoryIdAsync(int repositoryId);

    Task<List<PullRequest>> GetByAuthorIdAsync(int authorId);
}
