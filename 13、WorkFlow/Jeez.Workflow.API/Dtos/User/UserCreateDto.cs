namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户模型创建Dto
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string? Account { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string? JobNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string? HeadImg { get; set; }
    }
}
