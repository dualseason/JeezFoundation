namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程操作历史模型集合查询结果Dto
    /// </summary>
    public class WorkflowOperationHistoryDto
    {
        public string? InstanceId { get; set; }
        /// <summary>
        /// 实例编号
        /// </summary>
        public string? InstanceCode { get; set; }
        /// <summary>
        /// 实例状态
        /// </summary>
        public int? Status { set; get; }
        /// <summary>
        /// 发起人
        /// </summary>
        public string? CreateUserName { get; set; }
        /// <summary>
        /// 工作流Id
        /// </summary>
        public string? FlowId { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string? FlowName { get; set; }
        /// <summary>
        /// 工作流表单Id
        /// </summary>
        public string? FormId { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        public string? FormName { get; set; }
        /// <summary>
        /// 表单类型
        /// </summary>
        public int FormType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { set; get; } 
    }
}
