using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IOrganizationRepository : IGenericRepository<Organization>
{
    Task<List<Organization>> GetByOwnerIdAsync(int ownerId);

    Task<Organization?> GetByNameAsync(string name);

    Task<bool> ExistsByNameAsync(string name);
}
