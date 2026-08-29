namespace Domain.Common;

public class SoftDeleteEntity<Tkey> : BaseEntity<Tkey>, ISoftDelete
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }
}
