using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Collections.Generic;

namespace 内存缓存
{
    public class MemoryCacheHelper : IMemoryCacheHelper
    {
        private readonly IMemoryCache memoryCache;
        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        private void ValidateValueType<TResult>()
        {
            Type typeResult = typeof(TResult);
            if (typeResult.IsGenericType)
            {
                typeResult = typeResult.GetGenericTypeDefinition();
            }
            if(typeResult == typeof(IEnumerable<>) || typeResult == typeof(IEnumerable)
                || typeResult == typeof(IAsyncEnumerable<>)
                || typeResult == typeof(IQueryable<>) || typeResult == typeof(IQueryable))
            {
                throw new InvalidOperationException($"TResult of {typeResult} is not allowed, please use List<T> or T[] instead.");
            }
        }

        private void InitCacheEntry(ICacheEntry entry, int expireSeconds)
        {
            TimeSpan timespan = TimeSpan.FromSeconds(Random.Shared.Next(expireSeconds, expireSeconds * 2));
            entry.AbsoluteExpirationRelativeToNow = timespan;
        }

        public TResult? GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult?> valueFactory, int expireSeconds = 60) where TResult : ICollection
        {
            //ValidateValueType<TResult>(); //采用泛型约束
            if(!memoryCache.TryGetValue(cacheKey,out TResult? result))
            {
                using var entry = memoryCache.CreateEntry(cacheKey);
                result = valueFactory(entry);
                InitCacheEntry(entry, expireSeconds);
                entry.Value = result;
            }
            return result;
        }

        public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<ICacheEntry, Task<TResult?>> valueFactory, int expireSeconds = 60) where TResult : ICollection
        {
            //ValidateValueType<TResult>(); //采用泛型约束
            if (!memoryCache.TryGetValue(cacheKey, out TResult? result))
            {
                using var entry = memoryCache.CreateEntry(cacheKey);
                result = await valueFactory(entry);
                InitCacheEntry(entry, expireSeconds);
                entry.Value = result;
            }
            return result;
        }

        public void Remove(string cacheKey)
        {
            memoryCache.Remove(cacheKey);
        }
    }
}
