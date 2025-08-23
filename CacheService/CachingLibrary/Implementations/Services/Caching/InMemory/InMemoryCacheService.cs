using CachingLibrary.Common.Abstractions.Exceptions.Caching;
using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching.InMemory;
using CachingLibrary.Common.Utilities.Exception;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CachingLibrary.Implementations.Services.Caching.InMemory
{
    public class InMemoryCacheService :IInMemoryCacheService
    {
        readonly IMemoryCache _memoryCache;
        public static List<string> _keys = new();
        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<string> GetKeys() // EF Core 7+ etmek olur
        {
           
            var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

            var coherentStateValue = coherentState!.GetValue(_memoryCache);

            var entriesCollection = coherentStateValue!.GetType().GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            var entriesCollectionValue = entriesCollection!.GetValue(coherentStateValue) as ICollection;

            var keys = new List<string>();

            if (entriesCollectionValue != null)
            {
                foreach (var item in entriesCollectionValue)
                {
                    var methodInfo = item.GetType().GetProperty("Key");

                    var val = methodInfo.GetValue(item);
                    keys.Add(val.ToString());
                }
            }
            return keys;
        }

        public T GetCachedData<T>(string cacheKey)
        {
            return (T)_memoryCache.Get(cacheKey);
        }

        public bool IsExist(string cacheKey)
        {
            bool isExistCheck = _memoryCache.TryGetValue(cacheKey, out _);
            return isExistCheck;
        }

        public bool RemoveByPattern(string pattern)
        {
            var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

            var coherentStateValue = coherentState.GetValue(_memoryCache);

            var entriesCollection = coherentStateValue.GetType().GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            var entriesCollectionValue = entriesCollection.GetValue(coherentStateValue) as ICollection;

            if (entriesCollectionValue != null)
            {
                foreach (var item in entriesCollectionValue)
                {
                    var methodInfo = item.GetType().GetProperty("Key");

                    var val = methodInfo?.GetValue(item);

                    if (val != null)
                    {
                        var keyValue = val.ToString().ToLower();
                        var willRemoveValue = pattern.ToLower();
                        if (keyValue.Contains(willRemoveValue))
                        {
                            _memoryCache.Remove(val.ToString());
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool RemoveCachedData(string cacheKey)
        {
            bool checkValue = IsExist(cacheKey);
            bool IsEmptyNull = string.IsNullOrEmpty(cacheKey);
            if (checkValue && !IsEmptyNull)
            {
                _memoryCache.Remove(cacheKey);
                _keys.Remove(cacheKey);
                return true;
            }
            else
            {
                if (IsEmptyNull)
                {
                    throw new ArgumentNullException(nameof(cacheKey));
                }
                else if (!checkValue)
                {
                    throw new NonExistCachedValueException(ExceptionMessages.NonExistCachedValue);
                }
                return false;
            }
        }

        public bool SetCacheValue<T>(string cacheKey, T value, DateTimeOffset absoluteTimeSecond)
        {
            if (string.IsNullOrEmpty(cacheKey)) { throw new InvalidCacheKeyException(cacheKey); }
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = absoluteTimeSecond;
            _keys.Add(cacheKey);
            T data = _memoryCache.Set(cacheKey, value, options);
            return data != null;
        }

        public bool SetCacheValue<T>(string cacheKey, T value, DateTimeOffset? absoluteTimeSecond, int slidingTimeSecond)
        {
            if (string.IsNullOrEmpty(cacheKey)) { throw new InvalidCacheKeyException(ExceptionMessages.InvalidCacheKey); }
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = absoluteTimeSecond;
            options.SlidingExpiration = TimeSpan.FromSeconds(slidingTimeSecond);
            _keys.Add(cacheKey);
            T data = _memoryCache.Set(cacheKey, value, options);
            return data != null;
        }

       
    }
}
