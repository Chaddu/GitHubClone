namespace Domain.Common;

public class AuditableEntity<Tkey> : BaseEntity<Tkey>, IAuditable
{
    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}
