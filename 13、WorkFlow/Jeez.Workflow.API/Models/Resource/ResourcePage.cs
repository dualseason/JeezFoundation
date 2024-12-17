namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 资源【菜单】模型分页模型
    /// </summary>
    public class ResourcePage
    {
        /// <summary>
        /// 1、资源【菜单】模型集合
        /// </summary>
        public IEnumerable<Resource> Resources { get; set; } = new List<Resource>();

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
