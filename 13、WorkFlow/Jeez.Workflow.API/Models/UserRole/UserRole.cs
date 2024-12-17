using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 用户角色关联模型
    /// </summary>
    [Table("sys_user_role")]
    public class UserRole
    {
        
        /// <summary>
        /// 主键
        /// </summary>
        [Key,Identity]
        public long Id { get; set; }
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
        
        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }
    }

    /// <summary>
    /// 用户角色关联模型-用户角色关联模型表映射
    /// </summary>
    public sealed class UserRoleMapper : ClassMapper<UserRole>
    {
        public UserRoleMapper()
        {
            // 1、映射到ydt_user_role
            Table("ydt_user_role");

            // 2、自动映射【字段和属性】
            AutoMap();
        }
    }
}