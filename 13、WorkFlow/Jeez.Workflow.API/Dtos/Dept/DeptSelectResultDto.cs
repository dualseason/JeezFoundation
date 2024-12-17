namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 部门查询结果Dto
    /// </summary>
    public class DeptSelectResultDto
    {
        public long DeptId { get; set; }
        public long SystemId { get; set; }
        public string? DeptName { get; set; }
        public string? DeptCode { get; set; }
        public bool IsDel { get; set; }
        public string? Memo { get; set; }
        
        // 父级部门
        public List<DeptDto>? ParentDepts { get; set; } 
    }
}
