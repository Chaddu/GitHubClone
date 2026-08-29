using Domain.Common;

namespace Domain.Entities;
public class Label : SoftDeleteEntity<int>
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;


    public int RepositoryId { get; set; }

    public Repository Repository { get; set; } = null!;


    public ICollection<IssueLabel> IssueLabels { get; set; } = [];
}
