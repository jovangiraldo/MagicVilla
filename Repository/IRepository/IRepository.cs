using System.Linq.Expressions;

namespace API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        List<T> GetEverythingElment(Expression<Func<T, bool>>? filter = null);
        public T GetOnlyOneElement(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        void RemoveData(T entity);
        void Save();

    }
}
