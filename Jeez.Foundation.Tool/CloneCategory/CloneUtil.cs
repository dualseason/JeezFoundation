using Newtonsoft.Json;

namespace Jeez.Foundation.Tool
{
    /// <summary>
    /// 静态工具类 CloneHelper，用于深度克隆对象
    /// </summary>
    public static class CloneUtil
    {
        /// <summary>
        /// 定义一个泛型方法，接受一个泛型参数 T，并返回一个 T 类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(T obj)
        {
            if (obj == null)
            {
                return default(T);
            }

            var serialized = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        /// <summary>
        /// 静态工具类 CloneHelper，用于异步深度克隆对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async Task<T> CloneAsync<T>(T obj)
        {
            if (obj == null)
            {
                return default(T);
            }

            var serialized = JsonConvert.SerializeObject(obj);
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(serialized));
        }
    }
}