namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 角色模型分页入参模型
    /// </summary>
    public class RoleGetListPage
    {
        public bool IsDel { get; set; }

        /// <summary>
        /// 当前页【1 2 3】
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小【10 20 30】
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 分页偏移量【0 10 20 30】
        /// </summary>
        /// <returns></returns>
        public int OffSet()
        {
            return (PageIndex-1) * PageSize; 
        }
    }
}
