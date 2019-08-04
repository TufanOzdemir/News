using Microsoft.EntityFrameworkCore;
using Models.Interfaces;
using Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Base.Services
{
    public class BaseRepository<TModel, CustomDbContext> : BaseGenericRepository<TModel, int, CustomDbContext>, IBaseRepository<TModel, CustomDbContext>
        where TModel : class, IDataEntity
        where CustomDbContext : DbContext
    {
    }
}
