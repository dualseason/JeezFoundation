namespace horoscope
{
    /// <summary>
    /// 抽象horoscope
    /// </summary>
    public abstract class Abstracthoroscope : AbstractCulture, Ihoroscope
    {
        /// <summary>
        /// 推移
        /// </summary>
        /// <param name="n">推移步数</param>
        /// <returns>推移后的horoscope</returns>
        public Ihoroscope Next(int n)
        {
            throw new System.NotImplementedException();
        }
    }
}