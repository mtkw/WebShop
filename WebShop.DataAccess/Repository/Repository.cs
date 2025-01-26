using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.DataAccess.Repository
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

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includProperties = null)
        {
            IQueryable<T> query = _dbSet;
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
            return query.ToListAsync();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter, string? includProperties = null)
        {
            IQueryable<T> query = _dbSet;
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

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includProperties = null)
        {

            IQueryable<T> query = _dbSet;
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
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet?.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includProperties = null)
        {
            IQueryable<T> query = _dbSet;
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
            return query.FirstOrDefault();
        }

    }
}
