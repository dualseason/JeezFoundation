namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 子系统模型创建Dto
    /// </summary>
    public class SystemsCreateDto
    {
        public string? SystemName { get; set; }
        public string? SystemCode { get; set; }
        public bool IsDel { get; set; }
        public string? Memo { get; set; }
        public int Sort { get; set; }
        public long CreateTime { get; set; }
        public long CreateUserId { get; set; }
        public long UpdateTime { get; set; }
    }
}
