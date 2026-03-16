using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces.Repositories;
using School.Infrastructure.Persistence.Context;

namespace School.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly SchoolDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(SchoolDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
    public IQueryable<T> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }
}