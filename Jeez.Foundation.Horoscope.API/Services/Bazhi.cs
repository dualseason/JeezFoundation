
using JeezFoundation.Horoscope.lunar;
using JeezFoundation.Horoscope.solar;

namespace Jeez.Foundation.Horoscope.API.Services
{
    /// <summary>
    /// 八字服务类
    /// </summary>
    public class BazhiService : IBaZhiService
    {
        public bool ContainsShenSha(string shenshaName, string date)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetChangShengStates(string date)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, Tuple<string, string>> GetDaYun(string date, bool isMale)
        {
            throw new NotImplementedException();
        }

        public string GetGanZhiDay(string date)
        {
            throw new NotImplementedException();
        }

        public string GetGanZhiHour(string date)
        {
            throw new NotImplementedException();
        }

        public string GetGanZhiMonth(string date)
        {
            throw new NotImplementedException();
        }

        public string GetGanZhiYear(string date)
        {
            int[] timeList = date.Split("-").Select(x => int.Parse(x)).ToArray();
            SolarTime solarTime = SolarTime.FromYmdHms(timeList[0], timeList[1], timeList[2], 0, 0, 0);
            LunarHour lunarHour = solarTime.GetLunarHour();
            return lunarHour.ToString();
        }

        public Dictionary<string, string> GetLiuNianAnalysis(string date, int targetYear)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string[]> GetShenSha(string date)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string[]> GetShiShenRelations(string date)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetShiShenStats(string date)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetWuXingDistribution(string date)
        {
            throw new NotImplementedException();
        }

        public string GetZhuWuXing(string pillar, string date)
        {
            throw new NotImplementedException();
        }
    }
}
