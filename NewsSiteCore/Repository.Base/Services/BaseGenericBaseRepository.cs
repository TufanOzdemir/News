using Extensions;
using Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Interfaces;
using Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Base.Services
{
    public class BaseGenericBaseRepository<TModel, TKey, CustomDbContext> : IBaseGenericRepository<TModel, TKey, CustomDbContext>
       where TModel : class, IDbEntity<TKey>
       where CustomDbContext : DbContext
    {
        public CustomDbContext Context { get; }
        public ICacheManager CacheManager { get; }
        public ILogger<IBaseGenericRepository<TModel, TKey, CustomDbContext>> Logger { get; }
        public DbSet<TModel> DbSet { get; }

        protected static readonly Expression<Func<TModel, TKey>> DefaultSortExpression = c => c.Id;
        protected static readonly Func<TModel, TKey> DefaultSortFunc = c => c.Id;

        public BaseGenericBaseRepository(
            CustomDbContext context,
            ICacheManager cacheManager,
            ILogger<IBaseGenericRepository<TModel, TKey, CustomDbContext>> logger
            )
        {
            Context = context;
            CacheManager = cacheManager;
            Logger = logger;
            DbSet = Context.Set<TModel>();
        }

        private async Task<List<U>> QueryableToListAsync<U>(IQueryable<U> queryable)
        {
            try
            {
                return await queryable.ToListAsync();
            }
            catch (Exception exc)
            {
                Logger.LogError(exc, $"Error on get list: {typeof(U).Name}.");
                return null;
            }
        }

        public async Task<(bool success, int count)> DeleteAsync(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                var objects = await QueryableToListAsync(DbSet.Where(predicate));
                if (objects != null)
                {
                    DbSet.RemoveRange(objects);
                    return (true, Context.SaveChanges());
                }
                return (true, 0);
            }
            catch (Exception exc)
            {
                Logger.LogError(exc, $"Error on delete {typeof(TModel).Name}.");
                return (false, 0);
            }
        }

        public async Task<TModel> FirstOrDefaultAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await QueryableToListAsync(DbSet.Where(predicate).OrderByDescending(DefaultSortExpression));
        }

        public async Task<List<TModel>> GetListAsync()
        {
            return await QueryableToListAsync(DbSet.OrderByDescending(DefaultSortExpression).AsNoTracking());
        }

        public async Task<List<TModel>> GetListFromCacheAsync()
        {
            var key = typeof(TModel).FullName;
            return CacheManager.GetObject<List<TModel>>(key) ?? CacheManager.Set(key, await GetListAsync());
        }

        public async Task<(bool success, int count)> SaveOrUpdateWithoutCatchAsync(TModel entity)
        {
            if (IsPersistent(entity))
            {
                DbSet.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                DbSet.Add(entity);
            }

            var changes = await Context.SaveChangesAsync();

            return (true, changes);
        }

        protected bool IsPersistent(IDbEntity<TKey> entity)
        {
            return !entity.Id.Equals(default(TKey));
        }

        public async Task<(bool success, int count)> SaveOrUpdateAsync(TModel entity)
        {
            try
            {
                return await SaveOrUpdateWithoutCatchAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error on update {typeof(TModel).Name}.");
                return (false, 0);
            }
        }
        public async Task<(bool success, int count)> BulkSaveOrUpdateAsync(List<TModel> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    await SaveOrUpdateWithoutCatchAsync(entity);
                }
                return (true, entities.Count);
            }
            catch (DbUpdateException duEx)
            {
                Logger.LogError(duEx, entities.ToJson());
                return (false, 0);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error on update {typeof(TModel).Name}.");
                return (false, 0);
            }
        }
    }
}
