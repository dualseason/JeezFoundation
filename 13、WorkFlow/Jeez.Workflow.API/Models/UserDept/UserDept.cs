using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models
{
    /// <summary>
    /// 用户部门关联模型
    /// </summary>
    [Table("sys_user_dept")]
    public class UserDept
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
        /// 部门ID
        /// </summary>
        public long DeptId { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }

        public UserDept()
        {
            CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public UserDept(long userId, long deptId)
        {
            UserId = userId;
            DeptId = deptId;
            CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}