using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models.Systems
{
    /// <summary>
    /// 子系统模型
    /// </summary>
    [Table("sys_system")]
    public class Systems
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public long SystemId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public required string SystemName { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public required string SystemCode { get; set; }

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
        /// 更新时间戳
        /// </summary>
        public long UpdateTime { get; set; }
    }
}
