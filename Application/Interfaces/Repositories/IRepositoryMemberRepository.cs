using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IRepositoryMemberRepository : IGenericRepository<RepositoryMember>
{
    Task<List<RepositoryMember>> GetByRepositoryIdAsync(int repositoryId);

    Task<List<RepositoryMember>> GetByUserIdAsync(int userId);

    Task<RepositoryMember?> GetMembershipAsync(
        int repositoryId,
        int userId);

    Task<bool> IsMemberAsync(
        int repositoryId,
        int userId);
}
