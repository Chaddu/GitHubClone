using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IOrganizationMemberRepository : IGenericRepository<OrganizationMember>
{
    Task<List<OrganizationMember>> GetByOrganizationIdAsync(int organizationId);

    Task<List<OrganizationMember>> GetByUserIdAsync(int userId);

    Task<OrganizationMember?> GetMembershipAsync(
        int organizationId,
        int userId);

    Task<bool> IsMemberAsync(
        int organizationId,
        int userId);
}
