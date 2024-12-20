namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 工作流获取权限系统数据模型创建Dto
    /// </summary>
    public class WorkflowsqlCreateDto
    {
        public string? Name { get; set; }
        public string? FlowId { get; set; }
        public string? FlowSQL { get; set; }
        public string? Param { get; set; }
        public bool SQLType { get; set; }
        public int Status { get; set; }
        public string? Remark { get; set; }
        public long CreateUserId { get; set; }
        public long CreateTime { get; set; }
    }
}
