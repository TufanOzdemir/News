using Interfaces.HelperInterfaces;
using Interfaces.ResultModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interfaces.RepositoryInterfaces
{
    public interface IBaseGenericRepository<TModel, TKey, CustomDbContext>
        where TModel : class, IDbEntity<TKey>
        where CustomDbContext : DbContext
    {
        CustomDbContext Context { get; }
        ICacheManager CacheManager { get; }
        ILogger<IBaseGenericRepository<TModel, TKey, CustomDbContext>> Logger { get; }
        DbSet<TModel> DbSet { get; }
        Task<Result<TModel>> FirstOrDefaultAsync(Expression<Func<TModel, bool>> predicate);
        Task<Result<string>> SaveOrUpdateAsync(TModel entity);
        Task<Result<List<TModel>>> GetListAsync(Expression<Func<TModel, bool>> predicate);
        Task<Result<List<TModel>>> GetListAsync();
        Task<Result<List<TModel>>> GetListFromCacheAsync();
        Task<Result<string>> DeleteAsync(Expression<Func<TModel, bool>> predicate);
    }
}
