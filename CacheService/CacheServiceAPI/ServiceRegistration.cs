using CachingLibrary.Common.Utilities.Options;

namespace CacheServiceAPI
{
    public static class ServiceRegistration
    {
        public static void AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CachingOptions>(options => configuration.GetSection("Caching").Bind(options));

        }
    }
}
