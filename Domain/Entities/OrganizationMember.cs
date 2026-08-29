using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;
public class OrganizationMember : BaseEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public OrganizationRole Role { get; set; }
}
