﻿using Interfaces.RepositoryInterfaces;
using Interfaces.ServiceInterfaces;
using Models.DbModels;
using Models.HelperModels;
using Models.Interfaces;

namespace Repository.Services
{
    public class NewsContextProvider: INewsContextProvider
    {
        public NewsContext Context => ServiceGetter.GetService<NewsContext>();

        public IBaseRepository<Notification, NewsContext> Notifications => GetRepository<Notification>();
        public IBaseRepository<City, NewsContext> Cities => GetRepository<City>();
        public IBaseRepository<Post, NewsContext> Posts => GetRepository<Post>();

        private IBaseRepository<T, NewsContext> GetRepository<T>() where T : class, IDbEntity<int> => ServiceGetter.GetService<IBaseRepository<T, NewsContext>>();
    }
}
