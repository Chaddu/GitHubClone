using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IRepositoryRepository : IGenericRepository<Repository>
{
    Task<Repository?> GetByNameAsync(string name);

    Task<List<Repository>> GetByOwnerIdAsync(int ownerId);

    Task<bool> ExistsByNameAsync(string name);
}
