namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 工作流模型创建Dto
    /// </summary>
    public class WorkflowCreateDto
    {
        public string? FlowId { get; set; }
        public string? FlowCode { get; set; }
        public string? CategoryId { get; set; }
        public string? FormId { get; set; }
        public string? FlowName { get; set; }
        public string? FlowContent { get; set; }
        public int FlowVersion { get; set; }
        public string? Memo { get; set; }
        public int Enable { get; set; }
        public int IsOld { get; set; }
    }
}
