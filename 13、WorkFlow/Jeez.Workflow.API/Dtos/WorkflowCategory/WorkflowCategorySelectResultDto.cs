namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程分类模型集合查询结果Dto
    /// </summary>
    public class WorkflowCategorySelectResultDto
    {
        public string? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Memo { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// 父级分类【集合显示】
        /// </summary>
        public List<WorkflowCategoryDto>? ParentDtos { set; get; }
    }
}
