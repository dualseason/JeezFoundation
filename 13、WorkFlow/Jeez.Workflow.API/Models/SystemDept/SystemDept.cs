using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models.SystemDept
{
    /// <summary>
    /// 系统部门关联表
    /// </summary>
    [Table("sys_system_dept")]
    public class SystemDept
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity]
        public long Id { get; set; }

        /// <summary>
        /// 系统ID
        /// </summary>
        public long SystemId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptId { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }

        public SystemDept()
        {
            CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public SystemDept(long systemId, long deptId)
        {
            SystemId = systemId;
            DeptId = deptId;
            CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
