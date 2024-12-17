using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 角色模型
    /// </summary>
    [Table("sys_role")]
    public class Role
    {
        
        /// <summary>
        /// 主键
        /// </summary>
        [Key,Identity]
        public long RoleId { get; set; }
        
        /// <summary>
        /// 部门ID
        /// </summary>
        public long SystemId { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string? Memo { get; set; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        
        /// <summary>
        /// 修改人
        /// </summary>
        public long UpdateUserId { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public long UpdateTime { get; set; }
    }

    /// <summary>
    /// 角色模型-角色模型表映射
    /// </summary>
    public sealed class RoleMapper : ClassMapper<Role>
    {
        public RoleMapper()
        {
            // 1、映射到ydt_role
            Table("sys_role");

            // 2、自动映射【字段和属性】
            AutoMap();
        }
    }
}