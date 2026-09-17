using Domain.Enums;

namespace Application.DTOs.Request;
public class CreateReviewRequest
{
    public ReviewState State { get; set; }
    public string? Comment { get; set; }
}
