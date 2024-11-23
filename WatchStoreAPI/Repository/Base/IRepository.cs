using System.Linq.Expressions;

namespace WatchStoreAPI.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        T SelecteOne(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllInclude(params string[] agers);

        Task AddAsyncEntity(T entity);
        void DeleteEntity(T entity);
        void UpdateEntity(T entity);


    }
}
