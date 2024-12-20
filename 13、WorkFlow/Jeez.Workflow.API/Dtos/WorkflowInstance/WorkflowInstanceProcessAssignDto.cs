using JeezFoundation.WorkFlow;

namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例执行-委托Dto
    /// </summary>
    public class WorkflowInstanceProcessAssignDto
    {
        /// <summary>
        /// 工作流Id
        /// </summary>
        public string? FlowId { get; set; }
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string? InstanceId { set; get; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string? UserId { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { set; get; }

        /// <summary>
        /// 委托信息
        /// </summary>
        public FlowAssign? FlowAssign { get; set; }

    }
}
