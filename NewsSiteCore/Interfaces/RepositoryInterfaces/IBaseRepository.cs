using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

namespace Interfaces.RepositoryInterfaces
{
    public interface IBaseRepository<TModel, CustomDbContext> : IBaseGenericRepository<TModel, int, CustomDbContext>
        where TModel : class, IDbEntity<int>
        where CustomDbContext : DbContext
    {

    }
}
