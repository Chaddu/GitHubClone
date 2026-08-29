using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IBranchRepository : IGenericRepository<Branch>
{
    Task<List<Branch>> GetByRepositoryIdAsync(int repositoryId);

    Task<Branch?> GetByNameAsync(int repositoryId, string name);

    Task<bool> ExistsByNameAsync(int repositoryId, string name);
}
