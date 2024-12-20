namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 工作流和工作流分类结果Dto
    /// </summary>
    public class WorkflowAndWorkflowCategoryDto
    {
        /// <summary>
        /// 工作流集合
        /// </summary>
       public List<WorkflowDto>? workflowDtos { get; set; } 
        
        /// <summary>
        /// 工作流分类集合
        /// </summary>
       public List<WorkflowCategoryDto>? workflowCategories { get; set; }
    }
}
