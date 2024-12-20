namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程通知节点模型分页查询结果Dto
    /// </summary>
    public class WorkflowNoticePageDto
    {
        /// <summary>
        /// 1、流程通知节点模型集合
        /// </summary>
        public List<WorkflowNoticeDto> WorkflowNotices { get; set; } = new List<WorkflowNoticeDto>();


        public int TotalPages 
        {
            get 
            { 
                return _TotalPages();
            }
        }
        /// <summary>
        /// 2、总页数
        /// </summary>
        private int _TotalPages()
        {
            // 1、总条数 / 每页的条数=总页数
            var TotalPage = TotalItems / PageSize;

            // 1.1、取模运算
            var Count = TotalItems % PageSize;
            if (Count > 0)
            {
                TotalPage++;
            }
            return TotalPage;
        }

        /// <summary>
        /// 3、总条数
        /// </summary>
        public int TotalItems { set; get; }

        /// <summary>
        /// 当前页【1 2 3】
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 5、每页显示的条数【默认显示10条】
        /// </summary>
        public int PageSize { set; get; }
    }
}
