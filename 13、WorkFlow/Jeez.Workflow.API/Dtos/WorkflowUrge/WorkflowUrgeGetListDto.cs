namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 催办记录模型集合查询Dto
    /// </summary>
    public class WorkflowUrgeGetListDto
    {
        /// <summary>
        /// 状态【1：删除 0：未删除】
        /// </summary>
        public bool IsDel { get; set; } 
    }
}
