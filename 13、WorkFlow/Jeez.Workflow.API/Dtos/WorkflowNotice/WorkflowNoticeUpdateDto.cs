namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程通知节点模型更新Dto
    /// </summary>
    public class WorkflowNoticeUpdateDto
    {
        public string? NoticeId { get; set; }
        public string? InstanceId { get; set; }
        public string? NodeId { get; set; }
        public string? NodeName { get; set; }
        public string? Maker { get; set; }
        public bool IsTransition { get; set; }
        public bool Status { get; set; }
        public bool IsRead { get; set; }
    }
}
