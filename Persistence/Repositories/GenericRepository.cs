using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class

{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;


    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public async Task AddASync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}

