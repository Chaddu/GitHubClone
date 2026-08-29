using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Persistence.Repositories;
public interface IActivityRepository : IGenericRepository<Activity>
{
    Task<List<Activity>> GetByUserIdAsync(int userId);

    Task<List<Activity>> GetByRepositoryIdAsync(int repositoryId);
}
