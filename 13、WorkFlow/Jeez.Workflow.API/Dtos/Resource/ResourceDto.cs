namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 资源【菜单】模型集合查询结果Dto
    /// </summary>
    public class ResourceDto
    {
        public long ResourceId { get; set; }
        public long SystemId { get; set; }
        public string? ResourceName { get; set; }
        public long ParentId { get; set; }
        public string? ResourceUrl { get; set; }
        public int Sort { get; set; }
        public string? ButtonClass { get; set; }
        public string? Icon { get; set; }
        public bool IsShow { get; set; }
        public long CreateUserId { get; set; }
        public long CreateTime { get; set; }
        public bool IsDel { get; set; }
        public string? Memo { get; set; }
        public bool IsButton { get; set; }
        public bool ButtonType { get; set; }
        public string? Path { get; set; }
        
        // 子资源【子菜单】
        public List<ResourceDto>? ChildResourceDtos { get; set; } 
    }
}
