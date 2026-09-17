using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IRepositoryStarRepository : IGenericRepository<RepositoryStar>
{
    Task<RepositoryStar?> GetByRepositoryAndUserAsync(
       int repositoryId,
       int userId);

    Task<List<RepositoryStar>> GetByRepositoryIdAsync(int repositoryId);

    Task<List<RepositoryStar>> GetByUserIdAsync(int userId);
}
