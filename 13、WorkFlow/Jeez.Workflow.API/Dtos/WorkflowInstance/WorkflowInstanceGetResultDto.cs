using JeezFoundation.WorkFlow;

namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 查看工作流实例Dto
    /// </summary>
    public class WorkflowInstanceGetResultDto
    {
        public string? FlowId { get; set; }
        public string? FlowName { get; set; }
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string? InstanceId { get; set; }
        public string? FormId { get; set; }
        public WorkFlowFormType FormType { get; set; }
        /// <summary>
        /// json格式
        /// </summary>
        public string? FormContent { get; set; }
        public string? FormUrl { get; set; }
        public string? FormData { get; set; }

        /// <summary>
        /// 可操作按钮集合 JadeFramework.WorkFlow.WorkFlowMenu集合
        /// </summary>
        public List<int>? Menus { get; set; }

        /// <summary>
        /// 流程信息
        /// </summary>
        public WorkFlowProcessFlowData? FlowData { get; set; }

    }
}
