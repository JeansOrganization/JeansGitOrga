using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace 配置系统集成
{
    public class DistributedCacheHelper : IDistributedCacheHelper
    {
        private readonly IDistributedCache cache;
        public DistributedCacheHelper(IDistributedCache cache)
        {
            this.cache = cache;
        }

        private void InitOptionsExpireSeconds(DistributedCacheEntryOptions options, int expireSeconds)
        {
            options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireSeconds);
        }

        public TResult? GetOrCreate<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, TResult?> valueFactory, int expireSeconds = 60)
        {
            string? resultString = cache.GetString(cacheKey);
            /* Redis如果遇到null数据存入缓存会将null转成字符串"null"存入缓存,
             * 取用时反序列化会将"null"转化成null,从而校验为null的话返回[没获取到数据],
             * 可以用来区分是缓存中不存在该数据还是缓存中存入[数据库不存在该数据]两种情况 */
            if (resultString == null)
            {
                var options = new DistributedCacheEntryOptions();
                var result = valueFactory(options);
                InitOptionsExpireSeconds(options, expireSeconds);
                cache.SetString(cacheKey, JsonSerializer.Serialize(result), options);
                return result;
            }
            else
            {
                //Refresh()方法可以手动刷新该键对应值的滑动过期时间，调用GetString()方法获取键值时也会触发相同的重置效果
                cache.Refresh(cacheKey);
                return JsonSerializer.Deserialize<TResult>(resultString);
            }
        }

        public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, Task<TResult?>> valueFactory, int expireSeconds = 60)
        {
            string? resultString = await cache.GetStringAsync(cacheKey);
            if (resultString == null)
            {
                var options = new DistributedCacheEntryOptions();
                var result = await valueFactory(options);
                InitOptionsExpireSeconds(options, expireSeconds);
                await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), options);
                return result;
            }
            else
            {
                cache.Refresh(cacheKey);
                return JsonSerializer.Deserialize<TResult>(resultString);
            }
        }

        public void Remove(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await cache.RemoveAsync(cacheKey);
        }
    }
}
