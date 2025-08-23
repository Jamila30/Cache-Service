using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching;
using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching.InMemory;
using CachingLibrary.Common.Utilities.Attributes;
using CachingLibrary.Common.Utilities.Options;
using CachingLibrary.Implementations.Services.Caching.InMemory;
using CachingLibrary.Implementations.Services.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CachingLibrary
{
    public static class ServiceRegistration
    {
        public static void AddCachingLibrary(this IServiceCollection services)
        {
            services.AddScoped<ICacheService, InMemoryCacheService>();
            services.AddScoped<CacheAttribute>();
            services.AddScoped<RemoveStoredCacheAttribute>();
            services.AddMemoryCache();

            #region Options Registrations
            services.AddOptions<CachingOptions>();
            #endregion
        }
    }
}
