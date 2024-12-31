namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 子系统模型创建Dto
    /// </summary>
    public class SystemsCreateDto
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string? SystemName { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        public string? SystemCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Memo { get; set; }
    }
}
