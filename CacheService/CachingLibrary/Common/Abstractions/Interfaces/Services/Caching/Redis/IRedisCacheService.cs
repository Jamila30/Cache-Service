using StackExchange.Redis;

namespace CachingLibrary.Common.Abstractions.Interfaces.Services.Caching.Redis
{
    public interface IRedisCacheService : ICacheService
    {
        long RemoveCachedData(RedisKey[] cacheKeys);
    }
}
