namespace JeezFoundation.Horoscope
{
    /// <summary>
    /// horoscope
    /// </summary>
    public interface Ihoroscope: ICulture
    {
        /// <summary>
        /// 推移
        /// </summary>
        /// <param name="n">推移步数</param>
        /// <returns>推移后的horoscope</returns>
        Ihoroscope Next(int n);
    }
}