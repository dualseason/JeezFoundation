using JeezFoundation.Core.Domain.Entities;

namespace Jeez.Workflow.API.Commons
{
    /// <summary>
    /// 系统用户 【封装的是Cookie中的用户信息】
    /// </summary>
    public class SystemUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? HeadImg { get; set; }

        /// <summary>
        /// 用户性别
        /// </summary>
        public UserSex Sex { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 其他属性
        /// </summary>
        public object? Other { get; set; }
    }
}