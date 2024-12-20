namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程表单模型创建Dto
    /// </summary>
    public class WorkflowFormCreateDto
    {
        public string? FormId { get; set; }
        public string? FormName { get; set; }
        public int FormType { get; set; }
        public string? Content { get; set; }
        public string? OriginalContent { get; set; }
        public string? FormUrl { get; set; }
    }
}
