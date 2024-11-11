using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebShop.Data;
using WebShop.Repository.IRepository;

namespace WebShop.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        private static IQueryable<T> ApplyFilterAndIncludes(IQueryable<T> query, Expression<Func<T, bool>>? filter, string? includProperties)
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includProperties))
            {
                foreach (var includeProp in includProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter, string? includProperties = null)
        {
            var query = ApplyFilterAndIncludes(_dbSet, filter, includProperties);
            return query;
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includProperties = null)
        {
            var query = ApplyFilterAndIncludes(_dbSet, filter, includProperties);
            return query.FirstOrDefault();
        }


        public void Remove(T entity)
        {
            _dbSet?.Remove(entity);
        }
    }
}
