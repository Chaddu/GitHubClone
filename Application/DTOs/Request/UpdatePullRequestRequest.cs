namespace Application.DTOs.Request;
public class UpdatePullRequestRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
