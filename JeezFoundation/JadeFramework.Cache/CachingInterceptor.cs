using AspectCore.DynamicProxy;
using AspectCore.Injector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JeezFoundation.Cache
{
    /// <summary>
    /// 一个拦截器，用于实现方法级别的缓存功能。
    /// </summary>
    public class CachingInterceptor : AbstractInterceptor
    {
        /// <summary>
        /// 从容器中注入的缓存提供者。
        /// </summary>
        [FromContainer]
        public ICachingProvider CacheProvider { get; set; }

        /// <summary>
        /// 用于连接缓存键各个部分的字符。
        /// </summary>
        private char _linkChar = ':';

        /// <summary>
        /// 异步执行拦截器逻辑。
        /// </summary>
        /// <param name="context">拦截的上下文信息。</param>
        /// <param name="next">下一个拦截器或目标方法的委托。</param>
        /// <returns>一个代表异步操作的任务。</returns>
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var qCachingAttribute = GetQCachingAttributeInfo(context.ServiceMethod);
            if (qCachingAttribute != null)
            {
                await ProceedCaching(context, next, qCachingAttribute);
            }
            else
            {
                await next(context);
            }
        }

        /// <summary>
        /// 获取方法上的 CachingAttribute 缓存属性信息。
        /// </summary>
        /// <param name="method">方法信息。</param>
        /// <returns>CachingAttribute 实例，如果没有则返回 null。</returns>
        private CachingAttribute GetQCachingAttributeInfo(MethodInfo method)
        {
            return method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;
        }

        /// <summary>
        /// 处理缓存逻辑。
        /// </summary>
        /// <param name="context">拦截的上下文信息。</param>
        /// <param name="next">下一个拦截器或目标方法的委托。</param>
        /// <param name="attribute">CachingAttribute 缓存属性。</param>
        /// <returns>一个代表异步操作的任务。</returns>
        private async Task ProceedCaching(AspectContext context, AspectDelegate next, CachingAttribute attribute)
        {
            var cacheKey = GenerateCacheKey(context);

            var cacheValue = CacheProvider.Get(cacheKey);
            if (cacheValue != null)
            {
                context.ReturnValue = cacheValue;
                return;
            }

            await next(context);

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                CacheProvider.Set(cacheKey, context.ReturnValue, TimeSpan.FromSeconds(attribute.AbsoluteExpiration));
            }
        }

        /// <summary>
        /// 生成缓存键。
        /// </summary>
        /// <param name="context">拦截的上下文信息。</param>
        /// <returns>生成的缓存键。</returns>
        private string GenerateCacheKey(AspectContext context)
        {
            var typeName = context.ServiceMethod.DeclaringType.Name;
            var methodName = context.ServiceMethod.Name;
            var methodArguments = this.FormatArgumentsToPartOfCacheKey(context.ServiceMethod.GetParameters());

            return this.GenerateCacheKey(typeName, methodName, methodArguments);
        }

        /// <summary>
        /// 生成缓存键。
        /// </summary>
        /// <param name="typeName">类名。</param>
        /// <param name="methodName">方法名。</param>
        /// <param name="parameters">参数列表。</param>
        /// <returns>生成的缓存键。</returns>
        private string GenerateCacheKey(string typeName, string methodName, IList<string> parameters)
        {
            var builder = new StringBuilder();

            builder.Append(typeName);
            builder.Append(_linkChar);

            builder.Append(methodName);
            builder.Append(_linkChar);

            foreach (var param in parameters)
            {
                builder.Append(param);
                builder.Append(_linkChar);
            }

            return builder.ToString().TrimEnd(_linkChar);
        }

        /// <summary>
        /// 格式化方法参数为缓存键的一部分。
        /// </summary>
        /// <param name="methodArguments">方法参数信息列表。</param>
        /// <param name="maxCount">最大参数数量，默认为5。</param>
        /// <returns>参数值的列表。</returns>
        private IList<string> FormatArgumentsToPartOfCacheKey(IList<ParameterInfo> methodArguments, int maxCount = 5)
        {
            return methodArguments.Select(this.GetArgumentValue).Take(maxCount).ToList();
        }

        /// <summary>
        /// 获取参数值，用于构建缓存键。
        /// </summary>
        /// <param name="arg">参数值。</param>
        /// <returns>参数值的字符串表示。</returns>
        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is ICachable)
                return ((ICachable)arg).CacheKey;

            return null;
        }
    }
}