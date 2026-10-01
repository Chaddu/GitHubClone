using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class LabelService(IUnitOfWork unitOfWork, IMapper mapper) : ILabelService    
{
    public async Task<Result<LabelResponse>> CreateAsync(int repositoryId, CreateLabelRequest request, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repo == null)
            return Result<LabelResponse>.Failure("Repository not found", ErrorType.NotFound);

        if (repo.OwnerId != currentUserId)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result<LabelResponse>.Failure("You are not a member of this repository.", ErrorType.Forbidden);

            if (membership.Permission == RepositoryPermission.Viewer)
                return Result<LabelResponse>.Failure("You don't have permission to create labels.", ErrorType.Forbidden);
        }

        var exists = await unitOfWork.Labels.ExistsByNameAsync(repositoryId, request.Name);

        if (exists)
            return Result<LabelResponse>.Failure("Label with the same name already exists", ErrorType.Conflict);

        var label = mapper.Map<Label>(request);

        label.RepositoryId = repositoryId;

        await unitOfWork.Labels.AddASync(label);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<LabelResponse>(label);

        return Result<LabelResponse>.Success(response);
    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var label = await unitOfWork.Labels.GetByIdAsync(id);

        if (label == null)
            return Result.Failure("Label not found", ErrorType.NotFound);

        var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(label.RepositoryId, currentUserId);

        if (membership == null)
            return Result.Failure("You don't have permission to delete this label.",ErrorType.Forbidden);

        if (membership.Permission != RepositoryPermission.Admin &&
            membership.Permission != RepositoryPermission.Maintainer)
            return Result.Failure("You don't have permission to delete this label.",ErrorType.Forbidden);

        unitOfWork.Labels.Delete(label);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<LabelResponse>> GetByNameAsync(int repositoryId, string name, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repo == null)
            return Result<LabelResponse>.Failure("Repository not found", ErrorType.NotFound);

        if (repo.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result<LabelResponse>.Failure("You do not have permission to view this repository.",ErrorType.Forbidden);  
        }

        var label = await unitOfWork.Labels.GetByNameAsync(repositoryId, name);

        if (label == null)
            return Result<LabelResponse>.Failure("Label not found", ErrorType.NotFound);

        var response = mapper.Map<LabelResponse>(label);

        return Result<LabelResponse>.Success(response);
    }

    public async Task<Result<List<LabelResponse>>> GetByRepositoryIdAsync(int repositoryId, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repo == null)
            return Result<List<LabelResponse>>.Failure("Repository not found", ErrorType.NotFound);
        if (repo.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result<List<LabelResponse>>.Failure("You do not have permission to view this repository.",ErrorType.Forbidden);
        }

        var labels = await unitOfWork.Labels.GetByRepositoryIdAsync(repositoryId);

        var response = mapper.Map<List<LabelResponse>>(labels);

        return Result<List<LabelResponse>>.Success(response);

    }

    public async Task<Result<LabelResponse>> UpdateAsync(int id, UpdateLabelRequest request, int currentUserId)
    {
        var label = await unitOfWork.Labels.GetByIdAsync(id);

        if (label == null)
            return Result<LabelResponse>.Failure("Label was not found.", ErrorType.NotFound);

        var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(label.RepositoryId, currentUserId);

        if (membership == null)
            return Result<LabelResponse>.Failure("You don't have permission to update this label.",ErrorType.Forbidden);

        if (membership.Permission != RepositoryPermission.Admin &&
            membership.Permission != RepositoryPermission.Maintainer)
        {
            return Result<LabelResponse>.Failure("You don't have permission to update this label.",ErrorType.Forbidden);
        }
        if (label.Name != request.Name)
        {
            var exists = await unitOfWork.Labels
                .ExistsByNameAsync(label.RepositoryId, request.Name);

            if (exists)
            {
                return Result<LabelResponse>.Failure(
                    "A label with this name already exists.",
                    ErrorType.Conflict);
            }
        }

        mapper.Map(request, label);

        unitOfWork.Labels.Update(label);

        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<LabelResponse>(label);

        return Result<LabelResponse>.Success(response);
    }
}
