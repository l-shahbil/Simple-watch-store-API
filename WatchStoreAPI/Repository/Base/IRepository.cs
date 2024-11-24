using System.Linq.Expressions;

namespace WatchStoreAPI.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        T SelecteOne(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllIncludeAsync(params string[] agers);

        Task AddAsyncEntity(T entity);
        void DeleteEntity(T entity);
        void UpdateEntity(T entity);


    }
}
