using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class RepositoryStarService(IUnitOfWork unitOfWork, IMapper mapper) : IRepositoryStarService
{
    public async Task<Result<List<RepositoryStarResponse>>> GetByRepositoryIdAsync(int repositoryId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repo == null)
            return Result<List<RepositoryStarResponse>>.Failure("Repository was not found", ErrorType.NotFound);

        var stars = await unitOfWork.RepositoryStars.GetByRepositoryIdAsync(repositoryId);

        var resposne = mapper.Map<List<RepositoryStarResponse>>(stars);

        return Result<List<RepositoryStarResponse>>.Success(resposne);
    }

    public async Task<Result<List<RepositoryStarResponse>>> GetByUserIdAsync(int userId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId);

        if (user == null)
            return Result<List<RepositoryStarResponse>>.Failure("User was not found", ErrorType.NotFound);

        var stars = await unitOfWork.RepositoryStars.GetByUserIdAsync(userId);

        var response = mapper.Map<List<RepositoryStarResponse>>(stars);

        return Result<List<RepositoryStarResponse>>.Success(response); 
    }

    public async Task<Result<RepositoryStarResponse>> StarAsync(int repositoryId, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repo == null)
            return Result<RepositoryStarResponse>.Failure("Repository was not found", ErrorType.NotFound);

        var existingStar = await unitOfWork.RepositoryStars.GetByRepositoryAndUserAsync(currentUserId, repositoryId);

        if (existingStar != null)
            return Result<RepositoryStarResponse>.Failure("Yoou have already starred this repository", ErrorType.Conflict);

        var star = new RepositoryStar
        {
            RepositoryId = repositoryId,
            UserId = currentUserId,
        };

        await unitOfWork.RepositoryStars.AddASync(star);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<RepositoryStarResponse>(star);

        return Result<RepositoryStarResponse>.Success(response);
    }

    public async Task<Result> UnstarAsync(int repositoryId, int currentUserId)
    {
        var star = await unitOfWork.RepositoryStars.GetByRepositoryAndUserAsync(repositoryId, currentUserId);

        if (star == null)
            return Result.Failure("You have not starred this repository", ErrorType.NotFound);

        unitOfWork.RepositoryStars.Delete(star);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Repository unstarred successfully.");

    }
}
