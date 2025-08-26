using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching;
using CachingLibrary.Common.Utilities.Attributes;
using CachingLibrary.Implementations.Services.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace CachingLibrary
{
    public static class ServiceRegistration
    {
        public static void AddCachingLibrary(this IServiceCollection services)
        {
            services.AddScoped<ICacheService,RedisCacheService>();
            services.AddScoped<CacheAttribute>();
            services.AddScoped<RemoveStoredCacheAttribute>();
            services.AddMemoryCache();

        }

    }
}
