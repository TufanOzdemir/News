using Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.HelperModels;
using Models.Interfaces;
using Repository.Base.Interfaces;

namespace Repository.Base.Services
{
    public class BaseGenericRepository<TModel, TKey, CustomDbContext> : BaseGenericBaseRepository<TModel, TKey, CustomDbContext>
        where TModel : class, IDbEntity<TKey>
        where CustomDbContext : DbContext
    {
        public BaseGenericRepository() : base(
                 ServiceGetter.GetService<CustomDbContext>(),
                 ServiceGetter.GetService<ICacheManager>(),
                 ServiceGetter.GetService<ILogger<IBaseGenericRepository<TModel, TKey, CustomDbContext>>>())
        { }
    }
}
