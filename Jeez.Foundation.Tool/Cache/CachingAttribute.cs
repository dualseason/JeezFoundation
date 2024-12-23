using System;

namespace JeezFoundation.Tool
{
    /// <summary>
    /// 一个自定义属性，用于指示方法的缓存策略。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置方法的绝对过期时间（以分钟为单位）。
        /// </summary>
        /// <value>
        /// 过期时间，表示从方法调用后多长时间缓存将过期。
        /// </value>
        public int AbsoluteExpiration { get; set; }
    }
}