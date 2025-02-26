﻿using System.Text;

namespace Jeez.Foundation.Tool.ConvertCategory
{
    /// <summary>
    /// 数据类型转化
    /// </summary>
    public static partial class Extension
    {
        #region 数据转换扩展

        /// <summary>
        /// 转换成Byte
        /// </summary>
        /// <param name="s">输入字符串</param>
        /// <returns></returns>
        public static byte ToByte(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0;

            byte.TryParse(s.ToString(), out byte result);
            return result;
        }

        /// <summary>
        /// 转换成short/Int16
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static short ToShort(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0;

            short.TryParse(s.ToString(), out short result);
            return result;
        }

        /// <summary>
        /// 转换成Int/Int32
        /// </summary>
        /// <param name="s"></param>
        /// <param name="round">是否四舍五入，默认false</param>
        /// <returns></returns>
        public static int ToInt(this object s, bool round = false)
        {
            if (s == null || s == DBNull.Value)
                return 0;

            if (s.GetType().IsEnum)
            {
                return (int)s;
            }

            if (s is bool b)
                return b ? 1 : 0;

            if (int.TryParse(s.ToString(), out int result))
                return result;

            var f = s.ToFloat();
            return round ? Convert.ToInt32(f) : (int)f;
        }

        /// <summary>
        /// 转换成Long/Int64
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToLong(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0L;

            long.TryParse(s.ToString(), out long result);
            return result;
        }

        /// <summary>
        /// 转换成Float/Single
        /// </summary>
        /// <param name="s"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static float ToFloat(this object s, int? decimals = null)
        {
            if (s == null || s == DBNull.Value)
                return 0f;

            float.TryParse(s.ToString(), out float result);

            if (decimals == null)
                return result;

            return (float)Math.Round(result, decimals.Value);
        }

        /// <summary>
        /// 转换成Double/Single
        /// </summary>
        /// <param name="s"></param>
        /// <param name="digits">小数位数</param>
        /// <returns></returns>
        public static double ToDouble(this object s, int? digits = null)
        {
            if (s == null || s == DBNull.Value)
                return 0d;

            double.TryParse(s.ToString(), out double result);

            if (digits == null)
                return result;

            return Math.Round(result, digits.Value);
        }

        /// <summary>
        /// 转换成Decimal
        /// </summary>
        /// <param name="s"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object s, int? decimals = null)
        {
            if (s == null || s == DBNull.Value) return 0m;

            decimal.TryParse(s.ToString(), out decimal result);

            if (decimals == null)
                return result;

            return Math.Round(result, decimals.Value);
        }

        /// <summary>
        /// 转换成DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object s)
        {
            if (s == null || s == DBNull.Value)
                return DateTime.MinValue;

            DateTime.TryParse(s.ToString(), out DateTime result);
            return result;
        }

        /// <summary>
        /// 转换成Date
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object s)
        {
            return s.ToDateTime().Date;
        }

        /// <summary>
        /// 转换成Boolean
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ToBool(this object s)
        {
            if (s == null) return false;
            s = s.ToString().ToLower();
            if (s.Equals(1) || s.Equals("1") || s.Equals("true") || s.Equals("是") || s.Equals("yes"))
                return true;
            if (s.Equals(0) || s.Equals("0") || s.Equals("false") || s.Equals("否") || s.Equals("no"))
                return false;

            Boolean.TryParse(s.ToString(), out bool result);
            return result;
        }

        /// <summary>
        /// 泛型转换，转换失败会抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T To<T>(this object s)
        {
            return (T)Convert.ChangeType(s, typeof(T));
        }

        #endregion 数据转换扩展

        #region 布尔转换

        /// <summary>
        /// 布尔值转换为字符串1或者0
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToIntString(this bool b)
        {
            return b ? "1" : "0";
        }

        /// <summary>
        /// 布尔值转换为整数1或者0
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ToInt(this bool b)
        {
            return b ? 1 : 0;
        }

        /// <summary>
        /// 布尔值转换为中文
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToZhCn(this bool b)
        {
            return b ? "是" : "否";
        }

        #endregion 布尔转换



        #region 字节转换

        /// <summary>
        /// 转换为16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes, bool lowerCase = true)
        {
            if (bytes == null)
                return string.Empty;

            var result = new StringBuilder();
            var format = lowerCase ? "x2" : "X2";
            for (var i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString(format));
            }

            return result.ToString();
        }

        /// <summary>
        /// 16进制转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[]? HexToBytes(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            var bytes = new byte[s.Length / 2];

            for (int x = 0; x < s.Length / 2; x++)
            {
                int i = (Convert.ToInt32(s.Substring(x * 2, 2), 16));
                bytes[x] = (byte)i;
            }

            return bytes;
        }

        /// <summary>
        /// 转换为Base64
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 时间戳转本时区日期时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertFromDateTimeOffset(this DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
                return dateTime.DateTime;
        }

        /// <summary>
        /// 时间戳转本时区日期时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(this string timeStamp)
        {
            DateTime dd = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0), DateTimeKind.Local);
            long longTimeStamp = long.Parse(timeStamp + "0000");
            TimeSpan ts = new TimeSpan(longTimeStamp);
            return dd.Add(ts);
        }

        /// <summary>
        /// 字符串转Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>

        public static Guid? ToGuid(this string guid)
        {
            try
            {
                return new Guid(guid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion 字节转换



        #region 数字转字符串前面补零

        public static string IntToString(this int parm, int bit, bool fore = true)
        {
            int max = (int)(Math.Pow(10, bit) - 1);
            if (parm >= 0)
            {
                if (parm > max)
                {
                    throw new Exception("越界，无法转换");
                }
            }
            else
            {
                if (parm < -max)
                {
                    throw new Exception("越界，无法转换");
                }
            }

            if (fore)
            {
                return parm.ToString().PadLeft(bit, '0');
            }
            else
            {
                return parm.ToString().PadRight(bit, '0');
            }
        }

        #endregion 数字转字符串前面补零
    }
}