namespace CachingLibrary.Common.Abstractions.Interfaces.Services.Caching.InMemory
{
    public interface IInMemoryCacheService :ICacheService
    {
        public List<string> GetKeys();
        public bool SetCacheValue<T>(string cacheKey, T value, DateTimeOffset? absoluteTimeSecond, int slidingTimeSecond);
    }
}
