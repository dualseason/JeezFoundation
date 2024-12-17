using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 子系统模型
    /// </summary>
    [Table("sys_systems")]
    public class Systems
    {
        
        /// <summary>
        /// 部门ID
        /// </summary>
        [Key,Identity]
        public long SystemId { get; set; }
        
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? SystemName { get; set; }
        
        /// <summary>
        /// 部门编码
        /// </summary>
        public string? SystemCode { get; set; }
        
        /// <summary>
        /// 是否删除 1:是，0：否
        /// </summary>
        public bool IsDel { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string? Memo { get; set; }
        
        /// <summary>
        /// 路径
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public long UpdateTime { get; set; }
    }

    /// <summary>
    /// 子系统模型-子系统模型表映射
    /// </summary>
    public sealed class SystemsMapper : ClassMapper<Systems>
    {
        public SystemsMapper()
        {
            // 1、映射到ydt_systems
            Table("sys_systems");

            // 2、自动映射【字段和属性】
            AutoMap();
        }
    }
}