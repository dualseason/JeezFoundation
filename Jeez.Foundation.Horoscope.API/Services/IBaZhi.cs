namespace Jeez.Foundation.Horoscope.API.Services
{
    /// <summary>
    /// 八字核心功能接口 
    /// </summary>
    public interface IBaZhiService
    {
        // 基础四柱功能 
        string GetGanZhiYear(string date);
        string GetGanZhiMonth(string date);
        string GetGanZhiDay(string date);
        string GetGanZhiHour(string date);

        // 五行属性系统 
        /// <summary>
        /// 获取八字全局五行分布 
        /// </summary>
        Dictionary<string, int> GetWuXingDistribution(string date);

        /// <summary>
        /// 获取指定柱的五行属性（天干+地支本气）
        /// </summary>
        string GetZhuWuXing(string pillar, string date);

        // 十神关系系统
        /// <summary>
        /// 获取四柱十神关系（以日干为基准）
        /// </summary>
        Dictionary<string, string[]> GetShiShenRelations(string date);

        /// <summary>
        /// 获取特定十神分布统计 
        /// </summary>
        Dictionary<string, int> GetShiShenStats(string date);

        // 神煞系统
        /// <summary>
        /// 获取主要吉凶神煞 
        /// </summary>
        Dictionary<string, string[]> GetShenSha(string date);

        /// <summary>
        /// 检查特定神煞是否存在 
        /// </summary>
        bool ContainsShenSha(string shenshaName, string date);

        // 十二长生系统
        /// <summary>
        /// 获取各柱长生状态（以日干为基准）
        /// </summary>
        Dictionary<string, string> GetChangShengStates(string date);

        // 大运流年系统 
        /// <summary>
        /// 获取排大运信息（含起运时间）
        /// </summary>
        Dictionary<int, Tuple<string, string>> GetDaYun(string date, bool isMale);

        /// <summary>
        /// 获取特定年份流年分析 
        /// </summary>
        Dictionary<string, string> GetLiuNianAnalysis(string date, int targetYear);
    }
}