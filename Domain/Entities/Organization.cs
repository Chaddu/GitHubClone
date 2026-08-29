using Domain.Common;

namespace Domain.Entities;
public class Organization : SoftDeleteEntity<int>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int OwnerId { get; set; }

    public User Owner { get; set; } = null!;

    public ICollection<OrganizationMember> Members { get; set; } = [];

    public ICollection<Repository> Repositories { get; set; } = [];
}
