using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Freelando.Dados.Repository.Base;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly FreelandoContext _context;
    public Repository(FreelandoContext context)
    {
        _context = context;
    }

    public virtual async Task Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }
    public async Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public async Task<IQueryable<T>> GetAll()
    {
        return await Task.FromResult(_context.Set<T>().AsQueryable());
    }
    public async Task<T> GetById(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(predicate);
    }

    public async Task Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Set<T>().Update(entity);
    }
}
