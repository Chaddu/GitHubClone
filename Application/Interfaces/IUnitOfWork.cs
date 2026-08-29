using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Repositories;

namespace Application.Interfaces;
public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IOrganizationRepository Organizations { get; }

    IOrganizationMemberRepository OrganizationMembers { get; }

    IRepositoryRepository Repositories { get; }

    IRepositoryMemberRepository RepositoryMembers { get; }

    IGenericRepository<RepositoryStar> RepositoryStars { get; }

    IBranchRepository Branches { get; }

    IIssueRepository Issues { get; }

    ILabelRepository Labels { get; }

    IIssueLabelRepository IssueLabels { get; }

    IPullRequestRepository PullRequests { get; }

    IReviewRepository Reviews { get; }

    ICommentRepository Comments { get; }

    INotificationRepository Notifications { get; }

    IActivityRepository Activities { get; }
    Task<int> SaveChangesAsync();
}
