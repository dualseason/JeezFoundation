namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 催办记录模型更新Dto
    /// </summary>
    public class WorkflowUrgeUpdateDto
    {
        public string? UrgeId { get; set; }
        public string? InstanceId { get; set; }
        public string? NodeId { get; set; }
        public string? NodeName { get; set; }
        public string? Sender { get; set; }
        public string? UrgeUser { get; set; }
        public int UrgeType { get; set; }
        public string? UrgeContent { get; set; }
    }
}
