namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程分类模型集合查询结果Dto
    /// </summary>
    public class WorkflowCategoryDto
    {
        public string? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? Memo { get; set; }
        public int Status { get; set; }
        
        /// <summary>
        /// 子分类Dto
        /// </summary>
        public List<WorkflowCategoryDto>? ChildWorkflowCategoryDtos { set; get; }
    }
}
