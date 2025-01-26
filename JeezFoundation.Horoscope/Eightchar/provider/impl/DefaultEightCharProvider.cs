using JeezFoundation.Horoscope.Lunar;

namespace JeezFoundation.Horoscope.Eightchar.Provider.Impl
{
    /// <summary>
    /// 默认的八字计算（晚子时算第二天）
    /// </summary>
    public class DefaultEightCharProvider : IEightCharProvider
    {
        /// <inheritdoc />
        public EightChar GetEightChar(LunarHour hour)
        {
            return new EightChar(hour.YearSixtyCycle, hour.MonthSixtyCycle, hour.DaySixtyCycle, hour.SixtyCycle);
        }
    }
}