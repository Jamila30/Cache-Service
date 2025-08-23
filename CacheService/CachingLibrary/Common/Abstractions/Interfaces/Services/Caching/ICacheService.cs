namespace CachingLibrary.Common.Abstractions.Interfaces.Services.Caching
{
    public interface ICacheService
    {
        T GetCachedData<T>(string cacheKey);
        bool RemoveCachedData(string cacheKey);
        bool RemoveByPattern(string pattern);
        bool IsExist(string cacheKey);
        bool SetCacheValue<T>(string cacheKey, T value, DateTimeOffset absoluteTimeSecond);
    }
}
