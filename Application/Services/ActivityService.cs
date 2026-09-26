using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ActivityService(IUnitOfWork unitOfWork, IMapper mapper) : IActivityService
{
    public async Task<Result<ActivityResponse>> GetByIdAsync(int id,int currentUserId)
    {
        var activity = await unitOfWork.Activities.GetByIdAsync(id);

        if (activity == null)
            return Result<ActivityResponse>.Failure("Activity was not found",ErrorType.NotFound);

        var repository = await unitOfWork.Repositories.GetByIdAsync(activity.RepositoryId);

        if (repository == null)
            return Result<ActivityResponse>.Failure("Repository was not found",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repository.Id, currentUserId);

            if (membership == null)
                return Result<ActivityResponse>.Failure("You do not have permission to view this activity.",ErrorType.Forbidden);
        }

        var response = mapper.Map<ActivityResponse>(activity);

        return Result<ActivityResponse>.Success(response);
    }

    public async Task<Result<List<ActivityResponse>>> GetByRepositoryIdAsync(int repositoryId,int currentUserId)
    {
        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository == null)
            return Result<List<ActivityResponse>>.Failure("Repository was not found",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result<List<ActivityResponse>>.Failure("You do not have permission to view this repository.",ErrorType.Forbidden);
        }

        var activities = await unitOfWork.Activities.GetByRepositoryIdAsync(repositoryId);

        var response = mapper.Map<List<ActivityResponse>>(activities);

        return Result<List<ActivityResponse>>.Success(response);
    }

    public async Task<Result<List<ActivityResponse>>> GetByUserIdAsync(int userId,int currentUserId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId);

        if (user is null)
            return Result<List<ActivityResponse>>.Failure("User was not found.",ErrorType.NotFound);

        var activities = await unitOfWork.Activities.GetByUserIdAsync(userId);

        var visibleActivities = new List<Activity>();

        foreach (var activity in activities)
        {
            var repository = await unitOfWork.Repositories.GetByIdAsync(activity.RepositoryId);

            if (repository == null)
                continue;

            if (repository.Visibility == RepositoryVisibility.Public)
            {
                visibleActivities.Add(activity);
                continue;
            }

            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repository.Id, currentUserId);

            if (membership != null)
                visibleActivities.Add(activity);
        }

        var response = mapper.Map<List<ActivityResponse>>(visibleActivities);

        return Result<List<ActivityResponse>>.Success(response);
    }
}