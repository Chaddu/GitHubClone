using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Notification>> GetByUserIdAsync(int userId)
    {
        return await _dbSet.Where(n => n.UserId == userId)
                       .ToListAsync();
    }

    public async Task<List<Notification>> GetUnreadByUserIdAsync(int userId)
    {
        return await _dbSet.Where(n => n.UserId == userId && !n.IsRead)
                       .ToListAsync();
    }
}
