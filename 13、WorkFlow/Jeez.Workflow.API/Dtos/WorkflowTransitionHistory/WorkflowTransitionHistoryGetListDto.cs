namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程流转历史模型集合查询Dto
    /// </summary>
    public class WorkflowTransitionHistoryGetListDto
    {
        /// <summary>
        /// 状态【1：删除 0：未删除】
        /// </summary>
        public bool IsDel { get; set; } 
    }
}
