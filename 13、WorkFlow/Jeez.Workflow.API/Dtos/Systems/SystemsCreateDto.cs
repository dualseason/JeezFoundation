namespace Jeez.Workflow.API.Dtos.Systems
{
    /// <summary>
    /// 子系统模型创建Dto
    /// </summary>
    public class SystemsCreateDto
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public required string SystemName { get; set; }
        /// <summary>
        /// 系统代码
        /// </summary>
        public required string SystemCode { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public required string Memo { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 创建用户编码
        /// </summary>
        public long CreateUserId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }
    }
}
