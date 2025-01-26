using JeezFoundation.Horoscope.Lunar;

namespace JeezFoundation.Horoscope.Eightchar.Provider
{
    /// <summary>
    /// 八字计算接口
    /// </summary>
    public interface IEightCharProvider
    {
        /// <summary>
        /// 八字
        /// </summary>
        /// <param name="hour">农历时辰</param>
        /// <returns>八字</returns>
        EightChar GetEightChar(LunarHour hour);
    }
}