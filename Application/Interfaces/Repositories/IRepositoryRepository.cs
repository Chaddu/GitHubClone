using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IRepositoryRepository : IGenericRepository<Repository>
{
    Task<Repository?> GetByNameAsync(string name);
    Task<List<Repository>> GetByOrganizationIdAsync(int organizationId);
    Task<List<Repository>> GetByOwnerIdAsync(int ownerId);

    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByNameAsync(string name, int? ownerId, int? organizationId);
}
