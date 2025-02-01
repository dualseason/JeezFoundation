using System;
using JeezFoundation.Horoscope.solar;

namespace JeezFoundation.Horoscope.eightchar.provider.impl
{
    /// <summary>
    /// 元亨利贞的童限计算
    /// </summary>
    public class China95ChildLimitProvider : AbstractChildLimitProvider
    {
        /// <inheritdoc />
        public override ChildLimitInfo GetInfo(SolarTime birthTime, SolarTerm term)
        {
            // 出生时刻和节令时刻相差的分钟数
            var minutes = Math.Abs(term.JulianDay.GetSolarTime().Subtract(birthTime)) / 60;
            var year = minutes / 4320;
            minutes %= 4320;
            var month = minutes / 360;
            minutes %= 360;
            var day = minutes / 12;

            return Next(birthTime, year, month, day, 0, 0, 0);
        }
    }
}