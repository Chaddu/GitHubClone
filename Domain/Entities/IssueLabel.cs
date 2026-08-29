using Domain.Common;

namespace Domain.Entities;
public class IssueLabel : BaseEntity<int>
{
    public int IssueId { get; set; }
    public Issue Issue { get; set; } = null!;

    public int LabelId { get; set; }
    public Label Label { get; set; } = null!;

}
