using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface ILabelRepository : IGenericRepository<Label>
{
    Task<List<Label>> GetByRepositoryIdAsync(int repositoryId);

    Task<Label?> GetByNameAsync(int repositoryId, string name);

    Task<bool> ExistsByNameAsync(int repositoryId, string name);
}
