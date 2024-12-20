namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例执行-退回Dto
    /// </summary>
    public class WorkflowInstanceProcessReSubmitDto
    {
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string? InstanceId { set; get; }
        /// <summary>
        /// 工作流Id
        /// </summary>
        public string? FlowId { set; get; }
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
