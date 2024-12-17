namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 资源查询结果Dto
    /// </summary>
    public class ResourceSelectResultDto
    {
        public long ResourceId { get; set; }
        public string? ResourceName { get; set; }
        public string? ResourceUrl { get; set; }
        public int Sort { get; set; }
        public string? ButtonClass { get; set; }
        public string? Icon { get; set; }
        public bool IsShow { get; set; }
        public bool IsDel { get; set; }
        public string? Memo { get; set; }
        public bool IsButton { get; set; }
        public bool ButtonType { get; set; }
        public string? Path { get; set; }

        /// <summary>
        /// 父级资源Dto
        /// </summary>
        public ResourceDto? ParentResourceDtoDto { set; get; }

        /// <summary>
        /// 按钮资源Dto
        /// </summary>
        public List<ResourceButtonDto>? ResourceButtonDtos { get; set; } 
    }
}
