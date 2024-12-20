namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例模型【根据流程运行流程】集合查询结果Dto
    /// </summary>
    public class WorkflowImageDto
    {
        /// <summary>
        /// 工作流ID
        /// </summary>
        public string? FlowId { get; set; }

        /// <summary>
        /// 流程JSON内容
        /// </summary>
        public string? FlowContent { get; set; }
    }
}
