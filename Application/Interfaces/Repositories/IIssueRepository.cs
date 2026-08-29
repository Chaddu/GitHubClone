using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IIssueRepository : IGenericRepository<Issue>
{
    Task<List<Issue>> GetByRepositoryIdAsync(int repositoryId);

    Task<List<Issue>> GetOpenIssuesByRepositoryIdAsync(int repositoryId);

    Task<List<Issue>> GetByAuthorIdAsync(int authorId);
}
