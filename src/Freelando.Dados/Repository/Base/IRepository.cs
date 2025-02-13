using System.Linq.Expressions;

namespace Freelando.Dados.Repository.Base;

public interface IRepository<T>
{
    Task<IQueryable<T>> GetAll();
    Task<T> GetById(Expression<Func<T, bool>> predicate);
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}
