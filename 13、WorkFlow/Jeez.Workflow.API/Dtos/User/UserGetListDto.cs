namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户模型集合查询Dto
    /// </summary>
    public class UserGetListDto
    {
       public bool IsDel { get; set; } // 状态【1：删除 0：未删除】
    }
}
