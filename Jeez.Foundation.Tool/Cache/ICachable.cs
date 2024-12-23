namespace JeezFoundation.Tool
{
    /// <summary>
    /// 定义了一个可缓存对象的接口，该接口规定了对象必须提供一个用于缓存的键。
    /// </summary>
    public interface ICachable
    {
        /// <summary>
        /// 获取该对象用于缓存的唯一键。
        /// </summary>
        /// <value>
        /// 一个字符串，用作缓存键。
        /// </value>
        string CacheKey { get; }
    }
}