using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 角色模型
    /// </summary>
    [Table("sys_role")]
    public class Role : ModelBase
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
    }
}