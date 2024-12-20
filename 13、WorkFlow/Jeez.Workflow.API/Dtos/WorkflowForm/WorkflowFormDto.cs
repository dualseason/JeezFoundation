namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程表单模型集合查询结果Dto
    /// </summary>
    public class WorkflowFormDto
    {
        public string? FormId { get; set; }
        public string? FormName { get; set; }
        public int FormType { get; set; }
        public string? Content { get; set; }
        public string? OriginalContent { get; set; }
        public string? FormUrl { get; set; }
    }
}
