using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.Extensions;

namespace QuakeAPI.Data.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T: class
    {
        protected readonly QuakeDbContext _context;

        public RepositoryBase(QuakeDbContext context)
        {
            _context = context;
        }

        public T Create(T entity) =>
            _context.Set<T>().Add(entity).Entity;

        public virtual void Delete(T entity) =>
            _context.Set<T>().Remove(entity);

        public virtual void DeleteMany(IEnumerable<T> entities) =>
            _context.Set<T>().RemoveRange(entities);

        public IQueryable<T> FindAll(bool asTracking) => 
            _context.Set<T>()
            .EnableAsTracking(asTracking);

        public IQueryable<T> FindByCondition(System.Linq.Expressions.Expression<Func<T, bool>> expression, bool asTracking) =>
            _context.Set<T>()
            .Where(expression)
            .EnableAsTracking(asTracking);
    }
}