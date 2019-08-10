using Extensions;
using Interfaces.HelperInterfaces;
using Interfaces.RepositoryInterfaces;
using Interfaces.ResultModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Interfaces.Enums.Common;

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

        private async Task<Result<List<U>>> QueryableToListAsync<U>(IQueryable<U> queryable)
        {
            Result<List<U>> result;
            try
            {
                result = new Result<List<U>>(await queryable.ToListAsync());
            }
            catch (Exception exc)
            {
                Logger.LogError(exc, $"Error on get list: {typeof(U).Name}.");
                result = new Result<List<U>>(false, ResultType.Error, $"Error on get list: {typeof(U).Name}.");
            }
            return result;
        }

        public async Task<Result<string>> DeleteAsync(Expression<Func<TModel, bool>> predicate)
        {
            Result<string> result;
            try
            {
                var objects = await QueryableToListAsync(DbSet.Where(predicate));
                int count = 0;
                if (objects.IsSuccess)
                {
                    DbSet.RemoveRange(objects.Data);
                    count = Context.SaveChanges();
                }
                result = new Result<string>(true, ResultType.Success, count.ToString());
            }
            catch (Exception exc)
            {
                Logger.LogError(exc, $"Error on delete {typeof(TModel).Name}.");
                result = new Result<string>(false, ResultType.Error, $"Error on delete {typeof(TModel).Name}.");
            }
            return result;
        }

        public async Task<Result<TModel>> FirstOrDefaultAsync(Expression<Func<TModel, bool>> predicate)
        {
            return new Result<TModel>(await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate));
        }

        public async Task<Result<List<TModel>>> GetListAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await QueryableToListAsync(DbSet.Where(predicate).OrderByDescending(DefaultSortExpression));
        }

        public async Task<Result<List<TModel>>> GetListAsync()
        {
            return await QueryableToListAsync(DbSet.OrderByDescending(DefaultSortExpression).AsNoTracking());
        }

        public async Task<Result<List<TModel>>> GetListFromCacheAsync()
        {
            var key = typeof(TModel).FullName;
            var cacheValue = CacheManager.GetObject<List<TModel>>(key);
            if (!cacheValue.IsSuccess)
            {
                var dbList = await GetListAsync();
                cacheValue = dbList;
                CacheManager.Set(key, cacheValue);
            }
            return cacheValue;
        }

        public async Task<Result<string>> SaveOrUpdateWithoutCatchAsync(TModel entity)
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

            return new Result<string>(true, ResultType.Success, changes.ToString());
        }

        protected bool IsPersistent(IDbEntity<TKey> entity)
        {
            return !entity.Id.Equals(default(TKey));
        }

        public async Task<Result<string>> SaveOrUpdateAsync(TModel entity)
        {
            try
            {
                return await SaveOrUpdateWithoutCatchAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error on update {typeof(TModel).Name}.");
                return new Result<string>(false, ResultType.Error, $"Error on update {typeof(TModel).Name}.");
            }
        }
        public async Task<Result<string>> BulkSaveOrUpdateAsync(List<TModel> entities)
        {
            Result<string> result;
            try
            {
                foreach (var entity in entities)
                {
                    await SaveOrUpdateWithoutCatchAsync(entity);
                }
                result = new Result<string>(true, ResultType.Success, entities.Count.ToString());
            }
            catch (DbUpdateException duEx)
            {
                Logger.LogError(duEx, entities.ToJson());
                result = new Result<string>(false, ResultType.Error, entities.ToJson().ToString());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error on update {typeof(TModel).Name}.");
                result = new Result<string>(false, ResultType.Error, $"Error on update {typeof(TModel).Name}.");
            }
            return result;
        }
    }
}
