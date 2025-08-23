using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching;

namespace CachingLibrary.Implementations.Services.Caching
{
    public class CacheService
    {
        readonly ICacheService _cacheService;

        public CacheService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public T GetCachedData<T>(string cacheKey)
         => _cacheService.GetCachedData<T>(cacheKey);

        public bool IsExist(string cacheKey)
        => _cacheService.IsExist(cacheKey);

        public bool RemoveCachedData(string cacheKey)
        => _cacheService.RemoveCachedData(cacheKey);

        public bool SetCacheValue<T>(string cacheKey, T value, DateTimeOffset absoluteTimeSecond)
        => _cacheService.SetCacheValue(cacheKey, value, absoluteTimeSecond);
    }
}
