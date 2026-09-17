using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System.Text.RegularExpressions;

namespace Application.Services;

public class RepositoryMemberService(IUnitOfWork unitOfWork, IMapper mapper) : IRepositoryMemberService
{
    public async Task<Result<RepositoryMemberResponse>> AddMemberAsync(int repositoryId, AddRepositoryMemberRequest request, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if(repo == null)
            return Result<RepositoryMemberResponse>.Failure("Repository not found", ErrorType.NotFound);

        var canManage = await CanManageMembersAsync(repo, currentUserId);

        if(!canManage)
            return Result<RepositoryMemberResponse>.Failure("You are not allowed to manage members of this repository", ErrorType.Forbidden);

        if(repo.OwnerId == request.UserId)
            return Result<RepositoryMemberResponse>.Failure("The owner of the repository cannot be added as a member", ErrorType.BadRequest);

        var alreadyMember = await unitOfWork.RepositoryMembers.IsMemberAsync(repositoryId, request.UserId);

        if (alreadyMember)
            return Result<RepositoryMemberResponse>.Failure(
                "User is already a member of this repository.",
                ErrorType.Conflict);

        var member = mapper.Map<RepositoryMember>(request);
        member.RepositoryId = repositoryId;

        await unitOfWork.RepositoryMembers.AddASync(member);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<RepositoryMemberResponse>(member);

        return Result<RepositoryMemberResponse>.Success(response);
    }

    public async Task<Result<List<RepositoryMemberResponse>>> GetMembersAsync(int repositoryId)
    {
        var repository = await unitOfWork.Repositories
           .GetByIdAsync(repositoryId);

        if (repository is null)
            return Result<List<RepositoryMemberResponse>>.Failure(
                "Repository was not found.",
                ErrorType.NotFound);

        var members = await unitOfWork.RepositoryMembers.GetByRepositoryIdAsync(repositoryId);

        var response = mapper.Map<List<RepositoryMemberResponse>>(members);

        return Result<List<RepositoryMemberResponse>>.Success(response);

    }

    public async Task<Result> RemoveMemberAsync(int repositoryId, int memberId, int currentUserId)
    {

        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if(repository == null)
            return Result.Failure("Repository not found", ErrorType.NotFound);

        var canMagage = await CanManageMembersAsync(repository, currentUserId);

        if (!canMagage)
            return Result.Failure("You are not allowed to manage members of this repository", ErrorType.Forbidden);

        var member = await unitOfWork.RepositoryMembers.GetByIdAsync(memberId);

        if(member == null || member.RepositoryId != repositoryId)
            return Result.Failure("Member not found in this repository", ErrorType.NotFound);


        unitOfWork.RepositoryMembers.Delete(member);
        await unitOfWork.SaveChangesAsync();


        return Result.Success("Repository member removed successfully.");
    }

    public async Task<Result> UpdatePermissionAsync(int repositoryId, int memberId, UpdateRepositoryMemberPermissionRequest request, int currentUserId)
    {
        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if(repository == null)
            return Result.Failure("Repository not found", ErrorType.NotFound);

        var canManage = await CanManageMembersAsync(repository, currentUserId);

        if(!canManage)
            return Result.Failure("You are not allowed to manage members of this repository", ErrorType.Forbidden);

        var member = await unitOfWork.RepositoryMembers.GetByIdAsync(memberId);

        if(member == null || member.RepositoryId != repositoryId)
            return Result.Failure("Member not found in this repository", ErrorType.NotFound);

        member.Permission = request.Permission;

        unitOfWork.RepositoryMembers.Update(member);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Repository member permission updated successfully.");

    }

    private async Task<bool> CanManageMembersAsync(Repository repository,int currentUserId)
    {
        if (repository.OwnerId == currentUserId)
            return true;
        

        if (repository.OrganizationId != null)
        {
            var organizationMembership = await unitOfWork.OrganizationMembers
                .GetMembershipAsync(
                    repository.OrganizationId.Value,
                    currentUserId);

            if (organizationMembership != null &&
                (organizationMembership.Role == OrganizationRole.Owner ||
                 organizationMembership.Role == OrganizationRole.Admin))
                return true;
        }

        var repositoryMembership = await unitOfWork.RepositoryMembers
            .GetMembershipAsync(repository.Id, currentUserId);

        return repositoryMembership?.Permission == RepositoryPermission.Admin;
    }
}
