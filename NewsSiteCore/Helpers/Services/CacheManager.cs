using Helpers.HelperModels;
using Helpers.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Models.HelperModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Services
{
    public class CacheManager : ICacheManager
    {
        protected IMemoryCache _cache;
        protected ILogger<CacheManager> _logger;

        public CacheManager(IMemoryCache cache, ILogger<CacheManager> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        protected readonly object _lockObject = new object();

        public Dictionary<string, DateTimeOffset> GetKeys(CacheKeySearchModel filter = null)
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var a = field.GetValue(_cache);
            var collection = a as ICollection;

            var items = new Dictionary<string, DateTimeOffset>();
            var utcNow = DateTime.UtcNow;

            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var typeCacheEntry = item.GetType();

                    var keyProperty = typeCacheEntry.GetProperty("Key");
                    var key = keyProperty.GetValue(item) as string;

                    var valueProperty = typeCacheEntry.GetProperty("Value");
                    var value = valueProperty.GetValue(item) as ICacheEntry;

                    if (value.AbsoluteExpiration?.UtcDateTime > utcNow)
                    {
                        items.Add(key as string, value.AbsoluteExpiration.Value);
                    }
                }
            }

            var queryable = items.AsQueryable();

            if (filter != null)
            {
                if (filter.KeyFilter != null)
                {
                    queryable = queryable.Where(c => c.Key.StartsWith(filter.KeyFilter));
                }

                if (filter.MinDateOffset != null)
                {
                    queryable = queryable.Where(c => c.Value > filter.MinDateOffset);
                }

                if (filter.MaxDateOffset != null)
                {
                    queryable = queryable.Where(c => c.Value < filter.MaxDateOffset);
                }
            }

            return queryable.OrderBy(c => c.Key).ToDictionary(c => c.Key, c => c.Value);
        }

        public string GenereteCacheKey<t>(params object[] paramList)
        {
            return paramList.Any() ? string.Concat(typeof(t).Name, "_", string.Join("_", paramList)) : typeof(t).Name;
        }

        public void Clear(string key)
        {
            _cache.Remove(key);
        }

        public void ClearAll()
        {
            _cache.Dispose();
        }

        public void Add<T>(string key, T value) where T : class
        {
            Get<T>(key).Add(value);
        }

        public bool Remove<T>(string key, T value) where T : class
        {
            return Get<T>(key).Remove(value);
        }

        public List<string> ClearStartsWith(string prefix)
        {
            var keys = GetKeys(new CacheKeySearchModel { KeyFilter = prefix }).Select(c => c.Key).ToList();
            foreach (var key in keys)
            {
                Clear(key);
            }
            return keys;
        }

        public int Count(string prefix)
        {
            return GetKeys(new CacheKeySearchModel { KeyFilter = prefix }).Count;
        }

        public T Set<T>(string key, T value) where T : class
        {
            var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTimeOffset.MaxValue)
                    .SetSlidingExpiration(TimeSpan.MaxValue)
                    .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, options);
            return value;
        }

        public CacheableList<T> Set<T>(string key, CacheableList<T> value) where T : class
        {
            var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTimeOffset.MaxValue)
                    .SetSlidingExpiration(TimeSpan.MaxValue)
                    .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, options);
            return value;
        }

        public CacheableList<T> Get<T>(string key) where T : class
        {
            _cache.TryGetValue(key, out CacheableList<T> value);
            return value;
        }

        public T Set<T>(string key, T value, TimeSpan absoluteExpiration) where T : class
        {
            var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(absoluteExpiration)
                    .SetSlidingExpiration(TimeSpan.MaxValue)
                    .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, options);
            return value;
        }

        public T Set<T>(string key, T value, DateTimeOffset dateTimeOffset) where T : class
        {
            var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(dateTimeOffset)
                    .SetSlidingExpiration(TimeSpan.MaxValue)
                    .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, options);
            return value;
        }

        public T Set<T>(TimeSpan absoluteExpiration, string key, T value) where T : class
        {
            var options = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(absoluteExpiration)
                    .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, options);
            return value;
        }

        private DateTime GetNextRecurrence(TimeSpan recurrenceTime, TimeZoneInfo zone)
        {
            //Convert Time Zone
            var dateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Local, zone);
            var tickDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, recurrenceTime.Hours, recurrenceTime.Minutes, recurrenceTime.Seconds);
            if (tickDateTime < dateTime)
            {
                tickDateTime = tickDateTime.AddDays(1);
            }

            return TimeZoneInfo.ConvertTime(tickDateTime, zone, TimeZoneInfo.Local);
        }

        public T SetRecurrence<T>(string key, T value, TimeSpan recurrence, TimeZoneInfo zone) where T : class
        {
            var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(GetNextRecurrence(recurrence, zone))
                    .SetSlidingExpiration(TimeSpan.MaxValue)
                    .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, options);
            return value;
        }

        public Result<T> GetObject<T>(string key) where T : class
        {
            Result<T> result;
            _cache.TryGetValue(key, out T value);
            if (value == null)
            {
                result = new Result<T>(false,Enums.Common.ResultType.Warning,"Not Found!");
            }
            else
            {
                result = new Result<T>(value);
            }
            return result;
        }


        public async Task ClearRemoteCacheAsync(string cacheKey, string actionName, string siteURL)
        {
            var headersToAdd = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("fx-validation-key", "trusted"),
                new KeyValuePair<string, string>("fx-cache-action-name", actionName),
                new KeyValuePair<string, string>("fx-cache-key", cacheKey)
            };

            foreach (var site in siteURL.Split(','))
            {
                await CallUrlWithoutProxyAsync($"{site.TrimEnd('/')}/Cache/ClearCache", headersToAdd, "POST");
            }
        }

        public async Task CallUrlWithoutProxyAsync(string url, List<KeyValuePair<string, string>> headersToAdd = null, string httpMethod = "GET")
        {
            try
            {
                var req = WebRequest.Create(url);
                req.Method = httpMethod;
                req.ContentLength = 0;
                req.Timeout = 100000;
                req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                req.Proxy = null; // Disabling Proxy Detection For Fasting Requesting
                if (headersToAdd != null)
                {
                    foreach (var headerItem in headersToAdd)
                    {
                        req.Headers.Add(headerItem.Key, headerItem.Value);
                    }
                }

                (await req.GetResponseAsync()).Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, url);
            }
        }
    }
}