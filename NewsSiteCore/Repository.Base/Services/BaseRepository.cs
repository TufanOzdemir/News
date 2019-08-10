using Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

namespace Repository.Base.Services
{
    public class BaseRepository<TModel, CustomDbContext> : BaseGenericRepository<TModel, int, CustomDbContext>, IBaseRepository<TModel, CustomDbContext>
        where TModel : class, IDataEntity
        where CustomDbContext : DbContext
    {
    }
}
