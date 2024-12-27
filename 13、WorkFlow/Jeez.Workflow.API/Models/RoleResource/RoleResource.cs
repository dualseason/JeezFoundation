using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 角色资源关联模型
    /// </summary>
    [Table("ydt_role_resource")]
    public class RoleResource : ModelBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key,Identity]
        public long Id { get; set; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
        
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
    }
}