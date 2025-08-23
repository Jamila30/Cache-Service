namespace CachingLibrary.Common.Utilities.Options
{
    public sealed class CachingOptions
    {
        public RedisCacheOption? Redis { get; set; }
    }

    public sealed  class RedisCacheOption{
        public string ConnectionString { get; set; } = null!;
    }
}
