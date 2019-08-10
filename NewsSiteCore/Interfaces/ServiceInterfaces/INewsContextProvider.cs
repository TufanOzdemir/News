using Interfaces.RepositoryInterfaces;
using Models.DbModels;

namespace Interfaces.ServiceInterfaces
{
    public interface INewsContextProvider
    {
        NewsContext Context { get; }

        IBaseRepository<Notification, NewsContext> Notifications { get; }
        IBaseRepository<City, NewsContext> Cities { get; }
        IBaseRepository<Post, NewsContext> Posts { get; }
    }
}
