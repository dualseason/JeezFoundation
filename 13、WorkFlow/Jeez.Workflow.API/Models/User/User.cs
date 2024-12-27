using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 用户模型
    /// </summary>
    [Table("sys_user")]
    public class User
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key,Identity]
        public long UserId { get; set; }
        
        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; set; }
        
        /// <summary>
        /// 用户名
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

        /// <summary>
        /// 是否删除 1:是，0：否
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 更新人Id
        /// </summary>
        public long UpdateUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }
    }
}