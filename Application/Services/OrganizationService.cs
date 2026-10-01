using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class OrganizationService(IUnitOfWork unitOfWork, IMapper mapper) : IOrganizationService
{
    public async Task<Result<OrganizationResponse>> CreateAsync(CreateOrganizationRequest request, int userId)
    {
        var exists = await unitOfWork.Organizations.ExistsByNameAsync(request.Name);

        if (exists)
            return Result<OrganizationResponse>.Failure("Organization with the same name already exists.", ErrorType.Conflict);

        var organization = mapper.Map<Organization>(request);

        organization.OwnerId = userId;

        await unitOfWork.Organizations.AddASync(organization);
        await unitOfWork.SaveChangesAsync();

        var membership = new OrganizationMember
        {
            OrganizationId = organization.Id,
            UserId = userId,
            Role = OrganizationRole.Owner
        };

        await unitOfWork.OrganizationMembers.AddASync(membership);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<OrganizationResponse>(organization);

        return Result<OrganizationResponse>.Success(response);
    }

    public async Task<Result> DeleteAsync(int id, int userId)
    {
        var organization = await unitOfWork.Organizations.GetByIdAsync(id);

        if (organization == null)
            return Result.Failure("Organization not found.", ErrorType.NotFound);

        if (organization.OwnerId != userId)
            return Result.Failure("You are not authorized to delete this organization.", ErrorType.Forbidden);

        unitOfWork.Organizations.Delete(organization);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Organization deleted successfully.");


    }

    public async Task<Result<OrganizationResponse>> GetByIdAsync(int id)
    {
        var organization = await unitOfWork.Organizations.GetByIdAsync(id);

        if (organization == null)
            return Result<OrganizationResponse>.Failure("Organization not found.", ErrorType.NotFound);

        var response = mapper.Map<OrganizationResponse>(organization);

        return Result<OrganizationResponse>.Success(response);
    }

    public async Task<Result<List<OrganizationResponse>>> GetByOwnerIdAsync(int ownerId)
    {
        var organizations = await unitOfWork.Organizations.GetByOwnerIdAsync(ownerId);

        if (organizations == null)
            return Result<List<OrganizationResponse>>.Failure("Organization not found.", ErrorType.NotFound);

        var response = mapper.Map<List<OrganizationResponse>>(organizations);

        return Result<List<OrganizationResponse>>.Success(response);
    }

    public async Task<Result<OrganizationResponse>> UpdateAsync(int id, UpdateOrganizationRequest request, int userId)
    {
        var organization = await unitOfWork.Organizations.GetByIdAsync(id);

        if (organization == null)
            return Result<OrganizationResponse>.Failure("Organization not found.", ErrorType.NotFound);

        if (organization.OwnerId != userId)
            return Result<OrganizationResponse>.Failure("You are not authorized to update this organization.", ErrorType.Forbidden);

        if (organization.Name != request.Name)
        {
            var exists = await unitOfWork.Organizations
                .ExistsByNameAsync(request.Name);

            if (exists)
            {
                return Result<OrganizationResponse>.Failure(
                    "Organization name is already taken.",
                    ErrorType.Conflict);
            }
        }

        mapper.Map(request, organization);
        unitOfWork.Organizations.Update(organization);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<OrganizationResponse>(organization);

        return Result<OrganizationResponse>.Success(response);

    }
}
