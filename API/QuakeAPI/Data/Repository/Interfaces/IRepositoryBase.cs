using System.Linq.Expressions;

namespace QuakeAPI.Data.Repository.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool asTracking);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asTracking);
        T Create(T entity);
        void Delete(T entity);
    }
}