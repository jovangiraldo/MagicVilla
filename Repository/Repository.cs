using API.Data;
using API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Repository
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            dbSet = _context.Set<T>();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            Save();
        }

        public List<T> GetEverythingElment(Expression<Func<T, bool>>? filter = null)
        {
            if (filter != null)
            {
              return  _context.Set<T>().Where(filter).ToList();
            }
          
            return _context.Set<T>().ToList();
        }

        public T? GetOnlyOneElement(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> queryable = tracked ? _context.Set<T>() : _context.Set<T>().AsNoTracking();

            if (filter!=null)
            {
                queryable = queryable.Where(filter);
                
            }

            return queryable.FirstOrDefault();
        }

        public void RemoveData(T entity)
        {
           dbSet.Remove(entity);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
            
        }
    }
}
