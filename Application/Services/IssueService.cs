using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class IssueService(IUnitOfWork unitOfWork, IMapper mapper) : IIssueService
{
    public async Task<Result<IssueResponse>> CreateAsync(int repositoryId, CreateIssueRequest request, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if(repo == null)
            return Result<IssueResponse>.Failure("Repository not found.", ErrorType.NotFound);

        var issue = mapper.Map<Issue>(request);

        issue.RepositoryId = repositoryId;
        issue.CreatorId = currentUserId;
        issue.Status = IssueStatus.Open;

        await unitOfWork.Issues.AddASync(issue);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<IssueResponse>(issue);

        return Result<IssueResponse>.Success(response);

    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(id);

        if(issue == null)
            return Result.Failure("Issue not found.", ErrorType.NotFound);

        if(issue.CreatorId != currentUserId)
            return Result.Failure("You don't have permission to delete this issue.", ErrorType.Forbidden);

        unitOfWork.Issues.Delete(issue);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<List<IssueResponse>>> GetByAuthorIdAsync(int authorId)
    {
        var issues = await unitOfWork.Issues.GetByAuthorIdAsync(authorId);

        if(issues == null)
            return Result<List<IssueResponse>>.Failure("No issues found for the given author ID.", ErrorType.NotFound);

        var response = mapper.Map<List<IssueResponse>>(issues);

        return Result<List<IssueResponse>>.Success(response);
    }

    public async Task<Result<IssueResponse>> GetByIdAsync(int id)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(id);

        if(issue == null)
            return Result<IssueResponse>.Failure("Issue not found.", ErrorType.NotFound);

        var response = mapper.Map<IssueResponse>(issue);

        return Result<IssueResponse>.Success(response);
    }


    public async Task<Result<List<IssueResponse>>> GetByRepositoryIdAsync(int repositoryId)
    {
        var issue = await unitOfWork.Issues.GetByRepositoryIdAsync(repositoryId);

        if(issue == null)
            return Result<List<IssueResponse>>.Failure("No issues found for the given repository ID.", ErrorType.NotFound);
        
        var response = mapper.Map<List<IssueResponse>>(issue);

        return Result<List<IssueResponse>>.Success(response);
    }

    public async Task<Result<List<IssueResponse>>> GetOpenIssuesAsync(int repositoryId)
    {
        var issues = await unitOfWork.Issues.GetOpenIssuesByRepositoryIdAsync(repositoryId);

        if(issues == null)
            return Result<List<IssueResponse>>.Failure("No open issues found for the given repository ID.", ErrorType.NotFound);

        var response = mapper.Map<List<IssueResponse>>(issues);

        return Result<List<IssueResponse>>.Success(response);
    }

    public async Task<Result<IssueResponse>> UpdateAsync(int id, UpdateIssueRequest request, int currentUserId)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(id);

        if(issue == null)
            return Result<IssueResponse>.Failure("Issue not found.", ErrorType.NotFound);

        if(issue.CreatorId != currentUserId)
            return Result<IssueResponse>.Failure("You don't have permission to update this issue.", ErrorType.Forbidden);

        mapper.Map(request, issue);

        unitOfWork.Issues.Update(issue);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<IssueResponse>(issue);

        return Result<IssueResponse>.Success(response);
    }
}
