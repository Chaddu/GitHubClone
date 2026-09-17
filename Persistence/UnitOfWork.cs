using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _users;
    private readonly IOrganizationRepository _organizations;
    private readonly IOrganizationMemberRepository _organizationMembers;
    private readonly IRepositoryRepository _repositories;
    private readonly IRepositoryMemberRepository _repositoryMembers;
    private readonly IRepositoryStarRepository _repositoryStars;
    private readonly IBranchRepository _branches;
    private readonly IIssueRepository _issues;
    private readonly ILabelRepository _labels;
    private readonly IIssueLabelRepository _issueLabels;
    private readonly IPullRequestRepository _pullRequests;
    private readonly IReviewRepository _reviews;
    private readonly ICommentRepository _comments;
    private readonly INotificationRepository _notifications;
    private readonly IActivityRepository _activities;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        _users = new UserRepository(_context);
        _organizations = new OrganizationRepository(_context);
        _organizationMembers = new OrganizationMemberRepository(_context);
        _repositories = new RepositoryRepository(_context);
        _repositoryMembers = new RepositoryMemberRepository(_context);
        _repositoryStars = new RepositoryStarRepository(_context);
        _branches = new BranchRepository(_context);
        _issues = new IssueRepository(_context);
        _labels = new LabelRepository(_context);
        _issueLabels = new IssueLabelRepository(_context);
        _pullRequests = new PullRequestRepository(_context);
        _reviews = new ReviewRepository(_context);
        _comments = new CommentRepository   (_context);
        _notifications = new NotificationRepository(_context);
        _activities = new ActivityRepository(_context);
    }

    public IUserRepository Users => _users;

    public IOrganizationRepository Organizations => _organizations;

    public IOrganizationMemberRepository OrganizationMembers =>
        _organizationMembers;

    public IRepositoryRepository Repositories => _repositories;

    public IRepositoryMemberRepository RepositoryMembers =>
        _repositoryMembers;

    public IRepositoryStarRepository RepositoryStars =>
        _repositoryStars;

    public IBranchRepository Branches => _branches;

    public IIssueRepository Issues => _issues;

    public ILabelRepository Labels => _labels;

    public IIssueLabelRepository IssueLabels => _issueLabels;

    public IPullRequestRepository PullRequests => _pullRequests;

    public IReviewRepository Reviews => _reviews;

    public ICommentRepository Comments => _comments;

    public INotificationRepository Notifications => _notifications;

    public IActivityRepository Activities => _activities;
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
