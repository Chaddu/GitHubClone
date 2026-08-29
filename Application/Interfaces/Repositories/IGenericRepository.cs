namespace Application.Interfaces.Repositories;
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddASync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
