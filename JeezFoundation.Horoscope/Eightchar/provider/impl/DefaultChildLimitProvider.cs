﻿using System;
using JeezFoundation.Horoscope.solar;

namespace JeezFoundation.Horoscope.eightchar.provider.impl
{
    /// <summary>
    /// 默认的童限计算
    /// </summary>
    public class DefaultChildLimitProvider : AbstractChildLimitProvider
    {
        /// <inheritdoc />
        public override ChildLimitInfo GetInfo(SolarTime birthTime, SolarTerm term)
        {
            // 出生时刻和节令时刻相差的秒数
            var seconds = Math.Abs(term.JulianDay.GetSolarTime().Subtract(birthTime));
            // 3天 = 1年，3天=60*60*24*3秒=259200秒 = 1年
            var year = seconds / 259200;
            seconds %= 259200;
            // 1天 = 4月，1天=60*60*24秒=86400秒 = 4月，85400秒/4=21600秒 = 1月
            var month = seconds / 21600;
            seconds %= 21600;
            // 1时 = 5天，1时=60*60秒=3600秒 = 5天，3600秒/5=720秒 = 1天
            var day = seconds / 720;
            seconds %= 720;
            // 1分 = 2时，60秒 = 2时，60秒/2=30秒 = 1时
            var hour = seconds / 30;
            seconds %= 30;
            // 1秒 = 2分，1秒/2=0.5秒 = 1分
            var minute = seconds * 2;

            return Next(birthTime, year, month, day, hour, minute, 0);
        }
    }
}