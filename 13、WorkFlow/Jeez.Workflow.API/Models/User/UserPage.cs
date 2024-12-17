namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 用户模型分页模型
    /// </summary>
    public class UserPage
    {
        /// <summary>
        /// 1、用户模型集合
        /// </summary>
        public IEnumerable<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// 2、总页数
        /// </summary>
        public int TotalPages { set; get; }

        /// <summary>
        /// 3、总条数
        /// </summary>
        public int TotalItems { set; get; }

        /// <summary>
        /// 4、当前页【1 2 3】
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 5、每页显示的条数【默认显示10条】
        /// </summary>
        public int PageSize { set; get; }
    }
}
