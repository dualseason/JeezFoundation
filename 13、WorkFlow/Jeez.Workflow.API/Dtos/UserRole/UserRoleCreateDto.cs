namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户角色关联模型创建Dto
    /// </summary>
    public class UserRoleCreateDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public long CreateTime { get; set; }
    }
}
