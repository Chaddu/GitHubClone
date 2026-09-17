using Domain.Enums;

namespace Application.DTOs.Response;
public class ReviewResponse
{
    public int Id { get; set; }
    public ReviewState State { get; set; }
    public string? Comment { get; set; }
    public int PullRequestId { get; set; }
    public int ReviewerId { get; set; }
}
