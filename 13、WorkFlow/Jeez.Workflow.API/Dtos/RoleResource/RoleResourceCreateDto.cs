namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 角色资源关联模型创建Dto
    /// </summary>
    public class RoleResourceCreateDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long ResourceId { get; set; }
        public long CreateTime { get; set; }
    }
}
