using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class IssueLabelRepository : GenericRepository<IssueLabel>, IIssueLabelRepository
{
    public IssueLabelRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsAsync(int issueId, int labelId)
    {
        return await _dbSet.AnyAsync(x => x.IssueId == issueId && x.LabelId == labelId);
    }

    public async Task<IssueLabel?> GetAsync(int issueId, int labelId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.IssueId == issueId && x.LabelId == labelId);
    }

    public async Task<List<IssueLabel>> GetByIssueIdAsync(int issueId)
    {
        return await _dbSet.Where(x => x.IssueId == issueId).ToListAsync();
    }

    public async Task<List<IssueLabel>> GetByLabelIdAsync(int labelId)
    {
        return await _dbSet.Where(x => x.LabelId == labelId).ToListAsync();
    }
}
