using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;
public class RepositoryMember : AuditableEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int RepositoryId { get; set; }
    public Repository Repository { get; set; } = null!;

    public RepositoryPermission Permission { get; set; }
}
