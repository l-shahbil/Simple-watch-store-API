using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using WatchStoreAPI.Data;
using WatchStoreAPI.Repository.Base;

namespace WatchStoreAPI.Repository
{
    public class ImplementRepo<T> : IRepository<T> where T : class
    {
        private readonly appDbContext _dbContext;

        public ImplementRepo(appDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsyncEntity(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteEntity(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void UpdateEntity(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }

        public async Task<T> Get(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);

        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllInclude(params string[] agers)
        {
            var table = _dbContext.Set<T>();
            foreach (var ar in agers)
            {
                table.Include(ar);
            }
            return await table.ToListAsync();
        }

        public T SelecteOne(Expression<Func<T, bool>> filter)
        {
            return _dbContext.Set<T>().SingleOrDefault(filter);
        }


    }
}
