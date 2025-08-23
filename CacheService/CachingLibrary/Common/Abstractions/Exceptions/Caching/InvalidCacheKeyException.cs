using CachingLibrary.Common.Utilities.Exception;

namespace CachingLibrary.Common.Abstractions.Exceptions.Caching
{
    public class InvalidCacheKeyException :Exception
    {
        public static readonly int StatusCode = ExceptionCodes.InvalidCacheKey;
        public InvalidCacheKeyException() { }
        public InvalidCacheKeyException(string message) : base(message)
        {
        }
        public InvalidCacheKeyException(string message, Exception inner) : base(message, inner) { }
    }
}
