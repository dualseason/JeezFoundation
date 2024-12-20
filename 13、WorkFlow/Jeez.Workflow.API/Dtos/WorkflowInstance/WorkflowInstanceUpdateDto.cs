namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例模型【根据流程运行流程】更新Dto
    /// </summary>
    public class WorkflowInstanceUpdateDto
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        public string? InstanceId { get; set; }

        /// <summary>
        /// 表单数据
        /// </summary>
        public string? FormData { get; set; } 
     
    }
}
