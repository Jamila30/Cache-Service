using CachingLibrary.Common.Abstractions.Exceptions.Caching;
using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching.Redis;
using CachingLibrary.Common.Utilities.Exception;
using CachingLibrary.Common.Utilities.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CachingLibrary.Implementations.Services.Caching.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        private IDatabase _redisCache { get; set; }
        private CachingOptions _cachingOptions { get; set; }
        public ConnectionMultiplexer _redisConfiguration { get; }
        public RedisCacheService(IOptions<CachingOptions> cachingOptions)
        {
           _cachingOptions = cachingOptions.Value;
            _redisConfiguration = ConnectionMultiplexer.Connect(_cachingOptions!.Redis!.ConnectionString);
            _redisCache = _redisConfiguration.GetDatabase();
        }

        public T GetCachedData<T>(string cacheKey)
        {
            var result = _redisCache.StringGet(cacheKey);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<T>(result!)!;
            }
            return default;
        }

        public bool IsExist(string cacheKey)
        {
            return _redisCache.KeyExists(cacheKey);
        }

        public bool RemoveByPattern(string pattern)
        {
            /* Retrieving keys using the following line of code is not safe and 
             * may cause Redis to block in a production environment, especially when handling large datasets.
            */

            //var redisResultKeys = _redisCache.Execute("KEYS", "*");  

            var redisResultKeys= _redisConfiguration.GetServer(_cachingOptions!.Redis!.ConnectionString).Keys().ToArray(); 
            var patternMatchedKeys=redisResultKeys.Where(r => ((string)r).Contains(pattern)).ToArray();
            long countOfRemovedKeys = _redisCache.KeyDelete(patternMatchedKeys);
            return countOfRemovedKeys==patternMatchedKeys.Count();
        }

        public bool RemoveCachedData(string cacheKey)
        {
            var checkExist = _redisCache.KeyExists(cacheKey);
            if (checkExist)
            {
                return _redisCache.KeyDelete(cacheKey);
            }
            return false;
        }

        public long RemoveCachedData(RedisKey[] cacheKeys)
        {
            return _redisCache.KeyDelete(cacheKeys);
        }

        public bool SetCacheValue<T>(string cacheKey, T value, DateTimeOffset absoluteTimeSecond)
        {
            if (string.IsNullOrEmpty(cacheKey)) { throw new InvalidCacheKeyException(ExceptionMessages.InvalidCacheKey); }
            var expirtyDate = absoluteTimeSecond.UtcDateTime.Subtract(DateTime.UtcNow);
            return _redisCache.StringSet(cacheKey, JsonSerializer.Serialize(value), expirtyDate);
        }

      
    }
}
