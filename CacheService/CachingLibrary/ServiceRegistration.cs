using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching;
using CachingLibrary.Common.Utilities.Attributes;
using CachingLibrary.Common.Utilities.Options;
using CachingLibrary.Implementations.Services.Caching.InMemory;
using CachingLibrary.Implementations.Services.Caching.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

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

        public static void AddOptions(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<CachingOptions>(op => configuration.GetSection(nameof(CachingOptions)));

        }
    }
}
