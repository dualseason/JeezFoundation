namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 部门模型创建Dto
    /// </summary>
    public class DeptCreateDto
    {
        public long DeptId { get; set; }
        public long SystemId { get; set; }
        public string? DeptName { get; set; }
        public string? DeptCode { get; set; }
        public long ParentId { get; set; }
        public string? Path { get; set; }
        public bool IsDel { get; set; }
        public string? Memo { get; set; }
        public long CreateUserId { get; set; }
        public long CreateTime { get; set; }
    }
}
