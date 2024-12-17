using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 部门模型
    /// </summary>
    [Table("sys_dept")]
    public class Dept
    {
        
        /// <summary>
        /// 部门ID
        /// </summary>
        [Key,Identity]
        public long DeptId { get; set; }
        
        /// <summary>
        /// 部门ID
        /// </summary>
        public long SystemId { get; set; }
        
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DeptName { get; set; }
        
        /// <summary>
        /// 部门编码
        /// </summary>
        public string? DeptCode { get; set; }
        
        /// <summary>
        /// 父级ID
        /// </summary>
        public long ParentId { get; set; }
        
        /// <summary>
        /// 路径
        /// </summary>
        public string? Path { get; set; }
        
        /// <summary>
        /// 是否删除 1:是，0：否
        /// </summary>
        public bool IsDel { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string? Memo { get; set; }
        
        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserId { get; set; }
        
        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }
    }

    /// <summary>
    /// 部门模型-部门模型表映射
    /// </summary>
    public sealed class DeptMapper : ClassMapper<Dept>
    {
        public DeptMapper()
        {
            // 1、映射到ydt_dept
            Table("ydt_dept");

            // 2、自动映射【字段和属性】
            AutoMap();
        }
    }
}