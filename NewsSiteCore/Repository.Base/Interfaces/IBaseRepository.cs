using Microsoft.EntityFrameworkCore;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Base.Interfaces
{
    public interface IBaseRepository<TModel, CustomDbContext> : IBaseGenericRepository<TModel, int, CustomDbContext>
        where TModel : class, IDbEntity<int>
        where CustomDbContext : DbContext
    {

    }
}
