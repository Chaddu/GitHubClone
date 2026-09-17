using Domain.Enums;

namespace Application.DTOs.Request;

public class UpdateRepositoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RepositoryVisibility Visibility { get; set; }
}
