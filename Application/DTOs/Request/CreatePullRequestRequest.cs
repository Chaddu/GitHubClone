namespace Application.DTOs.Request;
public class CreatePullRequestRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SourceBranchId { get; set; }
    public int TargetBranchId { get; set; }
}
