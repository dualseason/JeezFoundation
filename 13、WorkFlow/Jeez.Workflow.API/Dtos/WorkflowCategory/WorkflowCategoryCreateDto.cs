namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程分类模型创建Dto
    /// </summary>
    public class WorkflowCategoryCreateDto
    {
        public string? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? Memo { get; set; }
        public int Status { get; set; }
    }
}
