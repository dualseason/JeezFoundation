using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 角色资源关联模型
    /// </summary>
    [Table("ydt_role_resource")]
    public class RoleResource
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
        
        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }
    }

    /// <summary>
    /// 角色资源关联模型-角色资源关联模型表映射
    /// </summary>
    public sealed class RoleResourceMapper : ClassMapper<RoleResource>
    {
        public RoleResourceMapper()
        {
            // 1、映射到ydt_role_resource
            Table("ydt_role_resource");

            // 2、自动映射【字段和属性】
            AutoMap();
        }
    }
}