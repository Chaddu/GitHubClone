using Domain.Common;

namespace Domain.Entities;

public class Notification : BaseEntity<int>
{
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }


    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
