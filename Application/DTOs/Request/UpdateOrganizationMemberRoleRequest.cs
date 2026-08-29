using Domain.Enums;

namespace Application.DTOs.Request;

public class UpdateOrganizationMemberRoleRequest
{
    public OrganizationRole Role { get; set; }
}
