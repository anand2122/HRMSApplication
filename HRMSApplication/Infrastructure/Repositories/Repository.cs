using HRMSApplication.Infrastructure.Data;
using HRMSApplication.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Linq.Expressions;

namespace MVC_DemoEFCore.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _demographicsDbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext demographicsDbContext)
        {
            _demographicsDbContext = demographicsDbContext;
            _dbSet = _demographicsDbContext.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate) => await  _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        //{
        //    return await _dbSet.ToListAsync();
        //}

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task RemoveAsync(int id) => await Task.Run(async ()=>_dbSet.Remove(await _dbSet.FindAsync(id)));

        public async Task UpdateAsync(T entity) => await Task.Run(()=> _dbSet.Update(entity));
    }
}
