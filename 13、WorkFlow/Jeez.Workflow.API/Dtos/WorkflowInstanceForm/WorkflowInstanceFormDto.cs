namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例表单关联模型集合查询结果Dto
    /// </summary>
    public class WorkflowInstanceFormDto
    {
        public string? InstanceFormId { get; set; }
        public string? InstanceId { get; set; }
        public string? FormId { get; set; }
        public string? FlowContent { get; set; }
        public int FormType { get; set; }
        public string? FormUrl { get; set; }
        public string? FormData { get; set; }
    }
}
