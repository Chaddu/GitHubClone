using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces.Services;
public interface IOrganizationService
{
    Task<Result<OrganizationResponse>> CreateAsync(CreateOrganizationRequest request,int userId);

    Task<Result<OrganizationResponse>> GetByIdAsync(int id);

    Task<Result<List<OrganizationResponse>>> GetByOwnerIdAsync(int ownerId);

    Task<Result<OrganizationResponse>> UpdateAsync(int id,UpdateOrganizationRequest request,int userId);

    Task<Result> DeleteAsync(int id,int userId);
}
