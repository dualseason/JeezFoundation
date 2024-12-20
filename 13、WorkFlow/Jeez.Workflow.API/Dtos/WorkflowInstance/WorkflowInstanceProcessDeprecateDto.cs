namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例执行-不同意Dto
    /// </summary>
    public class WorkflowInstanceProcessDeprecateDto
    {
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string? InstanceId { set; get; }
        /// <summary>
        /// 同意内容
        /// </summary>
        public string? DisagreeContent { set; get; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string? UserId { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { set; get; }
    }
}
