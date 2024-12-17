using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 资源【菜单】模型
    /// </summary>
    [Table("sys_resource")]
    public class Resource
    {
        
        /// <summary>
        /// 主键
        /// </summary>
        [Key,Identity]
        public long ResourceId { get; set; }
        
        /// <summary>
        /// 部门ID
        /// </summary>
        public long SystemId { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? ResourceName { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public long ParentId { get; set; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public string? ResourceUrl { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public string? ButtonClass { get; set; }
        
        /// <summary>
        /// 修改人
        /// </summary>
        public string? Icon { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public bool IsShow { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public long CreateUserId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public long CreateTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsDel { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? Memo { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsButton { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool ButtonType { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? Path { get; set; }
    }

    /// <summary>
    /// 资源【菜单】模型-资源【菜单】模型表映射
    /// </summary>
    public sealed class ResourceMapper : ClassMapper<Resource>
    {
        public ResourceMapper()
        {
            // 1、映射到ydt_resource
            Table("ydt_resource");

            // 2、自动映射【字段和属性】
            AutoMap();
        }
    }
}