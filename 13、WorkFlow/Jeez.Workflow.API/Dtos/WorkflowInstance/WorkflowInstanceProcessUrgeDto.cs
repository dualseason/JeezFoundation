namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例执行-催办Dto
    /// </summary>
    public class WorkflowInstanceProcessUrgeDto
    {
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string? InstanceId { set; get; }
        /// <summary>
        /// 催办内容
        /// </summary>
        public string? UrgeConent { set; get; }
        /// <summary>
        /// 催办类型
        /// </summary>
        public int UrgeType { set; get; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string? UserId { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { set; get; }
        /// <summary>
        /// 给谁催办
        /// </summary>
        public string? Sender { set; get; }

    }
}
