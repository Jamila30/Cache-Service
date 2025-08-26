namespace CachingLibrary.Common.Utilities.Options
{
    public  class CachingOptions
    {
        public RedisCache? RedisCache { get; set; }
    }

    public class RedisCache
    {
        public string? ConnectionString { get; set; } 
    }
}
