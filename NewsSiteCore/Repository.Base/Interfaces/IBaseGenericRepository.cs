using Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Base.Interfaces
{
    public interface IBaseGenericRepository<TModel, TKey, CustomDbContext>
        where TModel : class, IDbEntity<TKey>
        where CustomDbContext : DbContext
    {
        CustomDbContext Context { get; }
        ICacheManager CacheManager { get; }
        ILogger<IBaseGenericRepository<TModel, TKey, CustomDbContext>> Logger { get; }
        DbSet<TModel> DbSet { get; }
        Task<TModel> FirstOrDefaultAsync(Expression<Func<TModel, bool>> predicate);
        Task<(bool success, int count)> SaveOrUpdateAsync(TModel entity);
        Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> predicate);
        Task<List<TModel>> GetListAsync();
        Task<List<TModel>> GetListFromCacheAsync();
        Task<(bool success, int count)> DeleteAsync(Expression<Func<TModel, bool>> predicate);
    }
}
