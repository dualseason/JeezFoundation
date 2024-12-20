namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程流转历史模型更新Dto
    /// </summary>
    public class WorkflowTransitionHistoryUpdateDto
    {
        public string? TransitionId { get; set; }
        public string? InstanceId { get; set; }
        public string? FromNodeId { get; set; }
        public int FromNodeType { get; set; }
        public string? FromNodName { get; set; }
        public string? ToNodeId { get; set; }
        public int ToNodeType { get; set; }
        public string? ToNodeName { get; set; }
        public int TransitionState { get; set; }
        public int IsFinish { get; set; }
        public string? CreateUserName { get; set; }
    }
}
