using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IIssueLabelRepository : IGenericRepository<IssueLabel>
{
    Task<List<IssueLabel>> GetByIssueIdAsync(int issueId);

    Task<List<IssueLabel>> GetByLabelIdAsync(int labelId);

    Task<IssueLabel?> GetAsync(int issueId, int labelId);

    Task<bool> ExistsAsync(int issueId, int labelId);
}
