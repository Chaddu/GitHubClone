using Domain.Enums;

namespace Application.DTOs.Response;
public class PullRequestResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PullRequestStatus Status { get; set; }
    public int RepositoryId { get; set; }
    public int SourceBranchId { get; set; }
    public int TargetBranchId { get; set; }
    public int AuthorId { get; set; }

}
