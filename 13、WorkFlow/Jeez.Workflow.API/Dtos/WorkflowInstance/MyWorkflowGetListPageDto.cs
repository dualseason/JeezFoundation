namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例模型【根据流程运行流程】分页查询入参Dto
    /// </summary>
    public class MyWorkflowGetListPageDto
    {
        public int UserId { set; get; }
        public string? UserName { set; get; }

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
