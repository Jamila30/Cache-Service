using CachingLibrary.Common.Utilities.Exception;

namespace CachingLibrary.Common.Abstractions.Exceptions.Caching
{
    public class NonExistCachedValueException :Exception
    {
        public static readonly int StatusCode = ExceptionCodes.NonCacheKeyExist;
        public NonExistCachedValueException() { }
        public NonExistCachedValueException(string message) : base(message) { 
        }
        public NonExistCachedValueException(string message, Exception inner) : base(message, inner) { }
    }
}
