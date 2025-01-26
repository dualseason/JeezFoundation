using JeezFoundation.Horoscope.Lunar;

namespace JeezFoundation.Horoscope.Eightchar.Provider.Impl
{
    /// <summary>
    /// Lunar流派2的八字计算（晚子时日柱算当天）
    /// </summary>
    public class LunarSect2EightCharProvider : IEightCharProvider
    {
        /// <inheritdoc />
        public EightChar GetEightChar(LunarHour hour)
        {
            return new EightChar(hour.YearSixtyCycle, hour.MonthSixtyCycle, hour.LunarDay.SixtyCycle, hour.SixtyCycle);
        }
    }
}