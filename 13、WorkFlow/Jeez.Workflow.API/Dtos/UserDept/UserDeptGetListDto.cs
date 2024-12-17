namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户部门关联模型集合查询Dto
    /// </summary>
    public class UserDeptGetListDto
    {
        /// <summary>
        /// 状态【1：删除 0：未删除】
        /// </summary>
        public bool IsDel { get; set; } 
    }
}
