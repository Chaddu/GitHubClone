using Domain.Common;

namespace Domain.Entities;

public class Activity : BaseEntity<int>
{
    public string Action { get; set; } = string.Empty;


    public int UserId { get; set; }

    public User User { get; set; } = null!;


    public int RepositoryId { get; set; }

    public Repository Repository { get; set; } = null!;
}

