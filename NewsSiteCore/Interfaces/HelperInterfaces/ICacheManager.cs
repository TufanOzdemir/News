using Interfaces.ResultModel;
using Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.HelperInterfaces
{
    public interface ICacheManager
    {
        Dictionary<string, DateTimeOffset> GetKeys(CacheKeySearchModel filter = null);

        string GenereteCacheKey<t>(params object[] paramList);

        void Clear(string key);

        void ClearAll();

        void Add<T>(string key, T value) where T : class;

        bool Remove<T>(string key, T value) where T : class;

        List<string> ClearStartsWith(string key);

        int Count(string prefix);

        T Set<T>(string key, T value) where T : class;

        T Set<T>(TimeSpan absoluteExpiration, string key, T value) where T : class;

        T Set<T>(string key, T value, TimeSpan absoluteExpiration) where T : class;

        T Set<T>(string key, T value, DateTimeOffset dateTimeOffset) where T : class;

        T SetRecurrence<T>(string key, T value, TimeSpan recurrence, TimeZoneInfo zone) where T : class;

        Result<T> GetObject<T>(string key) where T : class;

        Task ClearRemoteCacheAsync(string cacheKey, string actionName, string siteURL);

        Task CallUrlWithoutProxyAsync(string url, List<KeyValuePair<string, string>> headersToAdd = null, string httpMethod = "GET");

        CacheableList<T> Set<T>(string key, CacheableList<T> value) where T : class;

        CacheableList<T> Get<T>(string key) where T : class;
    }
}
