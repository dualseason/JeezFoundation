using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models.DataPrivilege
{
    /// <summary>
    /// 数据权限表
    /// </summary>
    [Table("sys_data_privileges")]
    public class DataPrivilege
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        [Key, Identity]
        public long Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptId { get; set; }

        /// <summary>
        /// 系统ID
        /// </summary>
        public long SystemId { get; set; }
    }
}
