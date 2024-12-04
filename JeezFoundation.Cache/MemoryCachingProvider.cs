using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace JeezFoundation.Cache
{
    /// <summary>
    /// 实现了一个基于内存的缓存提供者，用于存储和检索缓存项。
    /// </summary>
    public class MemoryCachingProvider : ICachingProvider
    {
        /// <summary>
        /// 私有成员变量，用于存储内存缓存实例。
        /// </summary>
        private IMemoryCache _cache;

        /// <summary>
        /// 构造函数，用于初始化内存缓存提供者。
        /// </summary>
        /// <param name="cache">注入的内存缓存实例。</param>
        public MemoryCachingProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 根据缓存键获取缓存项。
        /// </summary>
        /// <param name="cacheKey">要检索的缓存项的键。</param>
        /// <returns>缓存项，如果不存在则返回null。</returns>
        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        /// <summary>
        /// 泛型方法，根据缓存键获取指定类型的缓存项。
        /// </summary>
        /// <typeparam name="TItem">缓存项的类型。</typeparam>
        /// <param name="key">要检索的缓存项的键。</param>
        /// <returns>指定类型的缓存项，如果不存在则返回null。</returns>
        public TItem Get<TItem>(object key) where TItem : class
        {
            return _cache.Get<TItem>(key);
        }

        /// <summary>
        /// 异步方法，根据缓存键获取缓存项。
        /// </summary>
        /// <param name="cacheKey">要检索的缓存项的键。</param>
        /// <returns>缓存项，如果不存在则返回null。</returns>
        public async Task<object> GetAsync(string cacheKey)
        {
            return await Task.FromResult<object>(_cache.Get(cacheKey));
        }

        /// <summary>
        /// 设置缓存项。
        /// </summary>
        /// <param name="cacheKey">缓存项的键。</param>
        /// <param name="cacheValue">要存储的缓存项的值。</param>
        public void Set(string cacheKey, object cacheValue)
        {
            _cache.Set(cacheKey, cacheValue);
        }

        /// <summary>
        /// 设置缓存项，并指定相对于当前时间的绝对过期时间。
        /// </summary>
        /// <param name="cacheKey">缓存项的键。</param>
        /// <param name="cacheValue">要存储的缓存项的值。</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于当前时间的绝对过期时间。</param>
        public void Set(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow)
        {
            _cache.Set(cacheKey, cacheValue, absoluteExpirationRelativeToNow);
        }

        /// <summary>
        /// 异步方法，设置缓存项，并指定相对于当前时间的绝对过期时间。
        /// </summary>
        /// <param name="cacheKey">缓存项的键。</param>
        /// <param name="cacheValue">要存储的缓存项的值。</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于当前时间的绝对过期时间。</param>
        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow)
        {
            await Task.Run(() =>
            {
                _cache.Set(cacheKey, cacheValue, absoluteExpirationRelativeToNow);
            });
        }
    }
}