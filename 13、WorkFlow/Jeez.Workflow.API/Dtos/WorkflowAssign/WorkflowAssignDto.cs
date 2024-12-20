namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程委托模型集合查询结果Dto
    /// </summary>
    public class WorkflowAssignDto
    {
        public string? AssignId { get; set; }
        public string? FlowId { get; set; }
        public string? InstanceId { get; set; }
        public string? NodeId { get; set; }
        public string? NodeName { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? AssignUserId { get; set; }
        public string? AssignUserName { get; set; }
        public string? Content { get; set; }
    }
}
