using JeezFoundation.WorkFlow;

namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例模型【根据流程运行流程】创建Dto
    /// </summary>
    public class WorkflowInstanceCreateDto
    {
        public string? FlowId { get; set; }
        public string? FlowName { get; set; }
        public string? FormId { get; set; }
        public WorkFlowFormType FormType { get; set; }
        public string? FormUrl { get; set; }
        public string? FormData { get; set; }
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
