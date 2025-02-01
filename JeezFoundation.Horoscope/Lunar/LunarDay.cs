﻿using System;
using System.Collections.Generic;
using JeezFoundation.Horoscope.culture;
using JeezFoundation.Horoscope.culture.fetus;
using JeezFoundation.Horoscope.culture.ren;
using JeezFoundation.Horoscope.culture.star.nine;
using JeezFoundation.Horoscope.culture.star.six;
using JeezFoundation.Horoscope.culture.star.twelve;
using JeezFoundation.Horoscope.culture.star.twentyeight;
using JeezFoundation.Horoscope.festival;
using JeezFoundation.Horoscope.sixtycycle;
using JeezFoundation.Horoscope.solar;

namespace JeezFoundation.Horoscope.lunar
{
    /// <summary>
    /// 农历日
    /// </summary>
    public class LunarDay : Abstracthoroscope
    {
        /// <summary>
        /// 名称
        /// </summary>
        public static string[] Names =
        {
            "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八",
            "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十"
        };

        /// <summary>
        /// 农历月
        /// </summary>
        public LunarMonth LunarMonth { get; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year => LunarMonth.Year;

        /// <summary>
        /// 月，闰月为负
        /// </summary>
        public int Month => LunarMonth.MonthWithLeap;

        /// <summary>
        /// 日
        /// </summary>
        public int Day { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="year">农历年</param>
        /// <param name="month">农历月，闰月为负</param>
        /// <param name="day">农历日</param>
        /// <exception cref="ArgumentException"></exception>
        public LunarDay(int year, int month, int day)
        {
            var m = LunarMonth.FromYm(year, month);
            if (day < 1 || day > m.DayCount)
            {
                throw new ArgumentException($"illegal day {day} in {m}");
            }

            LunarMonth = m;
            Day = day;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="year">农历年</param>
        /// <param name="month">农历月</param>
        /// <param name="day">农历日</param>
        /// <returns>农历日</returns>
        public static LunarDay FromYmd(int year, int month, int day)
        {
            return new LunarDay(year, month, day);
        }

        /// <summary>
        /// 名称
        /// </summary>
        /// <returns>名称</returns>
        public override string GetName()
        {
            return Names[Day - 1];
        }

        /// <summary>
        /// 完整描述
        /// </summary>
        /// <returns>完整描述</returns>
        public override string ToString()
        {
            return LunarMonth + GetName();
        }

        /// <summary>
        /// 推移
        /// </summary>
        /// <param name="n">推移步数</param>
        /// <returns>推移后的农历日</returns>
        public new LunarDay Next(int n)
        {
            return n == 0 ? FromYmd(Year, Month, Day) : GetSolarDay().Next(n).GetLunarDay();
        }

        /// <summary>
        /// 是否在指定农历日之前
        /// </summary>
        /// <param name="target">农历日</param>
        /// <returns>True/False</returns>
        public bool IsBefore(LunarDay target)
        {
            if (Year != target.Year)
            {
                return Year < target.Year;
            }

            if (Month != target.Month)
            {
                return Math.Abs(Month) < Math.Abs(target.Month);
            }

            return Day < target.Day;
        }

        /// <summary>
        /// 是否在指定农历日之后
        /// </summary>
        /// <param name="target">农历日</param>
        /// <returns>True/False</returns>
        public bool IsAfter(LunarDay target)
        {
            if (Year != target.Year)
            {
                return Year > target.Year;
            }

            if (Month != target.Month)
            {
                return Math.Abs(Month) >= Math.Abs(target.Month);
            }

            return Day > target.Day;
        }

        /// <summary>
        /// 星期
        /// </summary>
        public Week Week => GetSolarDay().Week;

        /// <summary>
        /// 当天的年干支（立春换）
        /// </summary>
        public SixtyCycle YearSixtyCycle
        {
            get
            {
                var solarDay = GetSolarDay();
                var solarYear = solarDay.Year;
                var springSolarDay = SolarTerm.FromIndex(solarYear, 3).JulianDay.GetSolarDay();
                var sixtyCycle = LunarMonth.LunarYear.SixtyCycle;
                if (Year == solarYear)
                {
                    if (solarDay.IsBefore(springSolarDay))
                    {
                        sixtyCycle = sixtyCycle.Next(-1);
                    }
                }
                else if (Year < solarYear)
                {
                    if (!solarDay.IsBefore(springSolarDay))
                    {
                        sixtyCycle = sixtyCycle.Next(1);
                    }
                }

                return sixtyCycle;
            }
        }

        /// <summary>
        /// 当天的月干支（节气换）
        /// </summary>
        public SixtyCycle MonthSixtyCycle
        {
            get
            {
                var solarDay = GetSolarDay();
                var year = solarDay.Year;
                var term = solarDay.Term;
                var index = term.Index - 3;
                if (index < 0 && term.JulianDay.GetSolarDay()
                        .IsAfter(SolarTerm.FromIndex(year, 3).JulianDay.GetSolarDay()))
                {
                    index += 24;
                }

                return LunarMonth.FromYm(year, 1).SixtyCycle.Next((int)Math.Floor(index * 1D / 2));
            }
        }

        /// <summary>
        /// 干支
        /// </summary>
        public SixtyCycle SixtyCycle
        {
            get
            {
                var offset = (int)LunarMonth.FirstJulianDay.Next(Day - 12).Day;
                return SixtyCycle.FromName(HeavenStem.FromIndex(offset).GetName() +
                                           EarthBranch.FromIndex(offset).GetName());
            }
        }

        /// <summary>
        /// 建除十二值神
        /// </summary>
        public Duty Duty => Duty.FromIndex(SixtyCycle.EarthBranch.Index - MonthSixtyCycle.EarthBranch.Index);

        /// <summary>
        /// 黄道黑道十二神
        /// </summary>
        public TwelveStar TwelveStar =>
            TwelveStar.FromIndex(SixtyCycle.EarthBranch.Index + (8 - MonthSixtyCycle.EarthBranch.Index % 6) * 2);

        /// <summary>
        /// 九星
        /// </summary>
        public NineStar NineStar
        {
            get
            {
                var solar = GetSolarDay();
                var dongZhi = SolarTerm.FromIndex(solar.Year, 0);
                var xiaZhi = dongZhi.Next(12);
                var dongZhi2 = dongZhi.Next(24);
                var dongZhiSolar = dongZhi.JulianDay.GetSolarDay();
                var xiaZhiSolar = xiaZhi.JulianDay.GetSolarDay();
                var dongZhiSolar2 = dongZhi2.JulianDay.GetSolarDay();
                var dongZhiIndex = dongZhiSolar.GetLunarDay().SixtyCycle.Index;
                var xiaZhiIndex = xiaZhiSolar.GetLunarDay().SixtyCycle.Index;
                var dongZhiIndex2 = dongZhiSolar2.GetLunarDay().SixtyCycle.Index;
                var solarShunBai = dongZhiSolar.Next(dongZhiIndex > 29 ? 60 - dongZhiIndex : -dongZhiIndex);
                var solarShunBai2 = dongZhiSolar2.Next(dongZhiIndex2 > 29 ? 60 - dongZhiIndex2 : -dongZhiIndex2);
                var solarNiZi = xiaZhiSolar.Next(xiaZhiIndex > 29 ? 60 - xiaZhiIndex : -xiaZhiIndex);
                var offset = 0;
                if (!solar.IsBefore(solarShunBai) && solar.IsBefore(solarNiZi))
                {
                    offset = solar.Subtract(solarShunBai);
                }
                else if (!solar.IsBefore(solarNiZi) && solar.IsBefore(solarShunBai2))
                {
                    offset = 8 - solar.Subtract(solarNiZi);
                }
                else if (!solar.IsBefore(solarShunBai2))
                {
                    offset = solar.Subtract(solarShunBai2);
                }
                else if (solar.IsBefore(solarShunBai))
                {
                    offset = 8 + solarShunBai.Subtract(solar);
                }

                return NineStar.FromIndex(offset);
            }
        }

        /// <summary>
        /// 太岁方位
        /// </summary>
        public Direction JupiterDirection => SixtyCycle.Index % 12 < 6
            ? Element.FromIndex(SixtyCycle.Index / 12).GetDirection()
            : LunarMonth.LunarYear.JupiterDirection;

        /// <summary>
        /// 逐日胎神
        /// </summary>
        public FetusDay FetusDay => FetusDay.FromLunarDay(this);

        /// <summary>
        /// 月相
        /// </summary>
        public Phase Phase => Phase.FromIndex(Day - 1);

        /// <summary>
        /// 六曜
        /// </summary>
        public SixStar SixStar => SixStar.FromIndex((LunarMonth.Month + Day - 2) % 6);

        /// <summary>
        /// 公历日
        /// </summary>
        /// <returns>公历日</returns>
        public SolarDay GetSolarDay()
        {
            return LunarMonth.FirstJulianDay.Next(Day - 1).GetSolarDay();
        }

        /// <summary>
        /// 二十八宿
        /// </summary>
        public TwentyEightStar TwentyEightStar => TwentyEightStar
            .FromIndex(new[] { 10, 18, 26, 6, 14, 22, 2 }[GetSolarDay().Week.Index])
            .Next(-7 * SixtyCycle.EarthBranch.Index);

        /// <summary>
        /// 农历传统节日，如果当天不是农历传统节日，返回null
        /// </summary>
        public LunarFestival Festival => LunarFestival.FromYmd(Year, Month, Day);

        /// <summary>
        /// 当天的时辰列表
        /// </summary>
        public List<LunarHour> Hours
        {
            get
            {
                var l = new List<LunarHour> { LunarHour.FromYmdHms(Year, Month, Day, 0, 0, 0) };
                for (var i = 0; i < 24; i += 2)
                {
                    l.Add(LunarHour.FromYmdHms(Year, Month, Day, i + 1, 0, 0));
                }

                return l;
            }
        }

        /// <summary>
        /// 神煞列表(吉神宜趋，凶神宜忌)
        /// </summary>
        /// <returns>神煞列表</returns>
        public List<God> Gods => God.GetDayGods(MonthSixtyCycle, SixtyCycle);

        /// <summary>
        /// 宜
        /// </summary>
        /// <returns>宜忌列表</returns>
        public List<Taboo> Recommends => Taboo.GetDayRecommends(MonthSixtyCycle, SixtyCycle);

        /// <summary>
        /// 忌
        /// </summary>
        /// <returns>宜忌列表</returns>
        public List<Taboo> Avoids => Taboo.GetDayAvoids(MonthSixtyCycle, SixtyCycle);

        /// <summary>
        /// 小六壬
        /// </summary>
        public MinorRen MinorRen => LunarMonth.MinorRen.Next(Day - 1);
    }
}