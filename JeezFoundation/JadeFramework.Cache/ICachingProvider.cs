using System;
using System.Threading.Tasks;

namespace JeezFoundation.Cache
{
    /// <summary>
    /// 定义了一个缓存提供者的接口，该接口规定了缓存操作的基本方法。
    /// </summary>
    public interface ICachingProvider
    {
        /// <summary>
        /// 根据提供的缓存键获取缓存项。
        /// </summary>
        /// <param name="cacheKey">要检索的缓存项的键。</param>
        /// <returns>缓存项，如果不存在则返回null。</returns>
        object Get(string cacheKey);

        /// <summary>
        /// 泛型方法，根据提供的键获取指定类型的缓存项。
        /// </summary>
        /// <typeparam name="TItem">缓存项的类型，必须是引用类型。</typeparam>
        /// <param name="key">要检索的缓存项的键。</param>
        /// <returns>指定类型的缓存项，如果不存在则返回null。</returns>
        TItem Get<TItem>(object key) where TItem : class;

        /// <summary>
        /// 异步方法，根据提供的缓存键获取缓存项。
        /// </summary>
        /// <param name="cacheKey">要检索的缓存项的键。</param>
        /// <returns>一个包含缓存项的任务对象，如果不存在则返回null。</returns>
        Task<object> GetAsync(string cacheKey);

        /// <summary>
        /// 设置缓存项。
        /// </summary>
        /// <param name="cacheKey">缓存项的键。</param>
        /// <param name="cacheValue">要存储的缓存项的值。</param>
        void Set(string cacheKey, object cacheValue);

        /// <summary>
        /// 设置缓存项，并指定相对于当前时间的绝对过期时间。
        /// </summary>
        /// <param name="cacheKey">缓存项的键。</param>
        /// <param name="cacheValue">要存储的缓存项的值。</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于当前时间的绝对过期时间。</param>
        void Set(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow);

        /// <summary>
        /// 异步方法，设置缓存项，并指定相对于当前时间的绝对过期时间。
        /// </summary>
        /// <param name="cacheKey">缓存项的键。</param>
        /// <param name="cacheValue">要存储的缓存项的值。</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于当前时间的绝对过期时间。</param>
        /// <returns>一个任务对象，表示设置缓存项的操作。</returns>
        Task SetAsync(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow);
    }
}