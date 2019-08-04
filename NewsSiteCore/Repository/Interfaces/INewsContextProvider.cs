using Models.DbModels;
using Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.News.Interfaces
{
    public interface INewsContextProvider
    {
        NewsContext Context { get; }

        IBaseRepository<Notification, NewsContext> Notifications { get; }
        IBaseRepository<City, NewsContext> Cities { get; }
        IBaseRepository<Post, NewsContext> Posts { get; }
    }
}
