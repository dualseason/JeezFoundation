namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 首页出参Dto
    /// </summary>
    public class IndexDto
    {
        public ApprovaItemDto ApprovaItem { get; set; } =new ApprovaItemDto();
    }

    /// <summary>
    /// 待办事项模型
    /// </summary>
    public class ApprovaItemDto
    {
        /// <summary>
        /// 待办数量
        /// </summary>
        public int ApprovaClount { set; get; }

        /// <summary>
        /// 待办集合
        /// </summary>
        public List<ApprovaDto>? ApprovaDtos { get; set; } = new List<ApprovaDto>();
    }

    /// <summary>
    /// 待办模型
    /// </summary>
    public class ApprovaDto
    {
        public string? FlowName { set; get; }
        public long CreateTime { set; get; }
        public string? CreateUserName { set; get; }
    }
}
