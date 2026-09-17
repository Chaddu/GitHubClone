using Domain.Enums;

namespace Application.DTOs.Response;

public class RepositoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RepositoryVisibility Visibility { get; set; }
    public int? OwnerId { get; set; }
    public int? OrganizationId { get; set; }
}
