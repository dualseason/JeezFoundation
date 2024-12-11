namespace Jeez.Workflow.API.Dtos.Systems
{
    /// <summary>
    /// 子系统模型集合查询Dto
    /// </summary>
    public class SystemsGetListDto
    {
        /// <summary>
        /// 状态【1：删除 0：未删除】
        /// </summary>
        public bool IsDel { get; set; }
    }
}
