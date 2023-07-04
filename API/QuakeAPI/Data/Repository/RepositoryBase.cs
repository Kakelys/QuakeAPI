using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO;

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

        public void Delete(T entity) =>
            _context.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool asTracking) => 
            asTracking ? 
                _context.Set<T>() : 
                _context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(System.Linq.Expressions.Expression<Func<T, bool>> expression, bool asTracking) =>
            asTracking ? 
                _context.Set<T>().Where(expression) : 
                _context.Set<T>().Where(expression).AsNoTracking();

        public IQueryable<T> FindPage(Page page, bool asTracking) => 
            asTracking ? 
                _context.Set<T>().Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize) : 
                _context.Set<T>().Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).AsNoTracking();
    }
}