using Domain.Enums;

namespace Application.DTOs.Response;

public class OrganizationMemberResponse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int OrganizationId { get; set; }

    public OrganizationRole Role { get; set; }
}
