namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户角色关联模型集合查询结果Dto
    /// </summary>
    public class UserRoleDto
    {
        public long UserId { get; set; }
        /// <summary>
        /// 系统Id
        /// </summary>
        public long SystemId { get; set; }
        /// <summary>
        /// 系统编码
        /// </summary>
        public string? SystemCode { get; set; }
        /// <summary>
        /// 系统Name
        /// </summary>
        public string? SystemName { get; set; }
        /// <summary>
        /// 用户角色集合
        /// </summary>
        public List<UserRoleList>? UserRoleLists { get; set; } 
    }

    /// <summary>
    /// 用户角色集合
    /// </summary>
    public class UserRoleList
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }
        /// <summary>
        /// 是否选中【true 代表选中，false 未选中】
        /// </summary>
        public bool Selected { set; get; } 
    }
}
