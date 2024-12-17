namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户角色关联模型更新Dto
    /// </summary>
    public class UserRoleUpdateDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public long CreateTime { get; set; }
    }
}
