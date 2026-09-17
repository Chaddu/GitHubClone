using Domain.Enums;

namespace Application.DTOs.Request;
public class UpdateReviewRequest
{
    public ReviewState State { get; set; }
    public string? Comment { get; set; }

}
