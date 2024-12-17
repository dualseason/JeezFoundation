namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 资源【菜单】模型创建Dto
    /// </summary>
    public class ResourceCreateDto
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

        // 按钮Dto
        public  List<ResourceButtonDto>? ResourceButtonsDtos { get; set; }   
    }

    /// <summary>
    /// 资源按钮Dto
    /// </summary>
    public class ResourceButtonDto
    {
        /// <summary>
        /// 资源主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 按钮类型 ==>用于vue
        /// </summary>
        public byte ButtonModel { get; set; }
        /// <summary>
        ///  按钮名称
        /// </summary>
        public string? Name { get; set; }
    }
}
