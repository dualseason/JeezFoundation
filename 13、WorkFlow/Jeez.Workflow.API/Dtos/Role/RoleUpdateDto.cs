namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 角色模型更新Dto
    /// </summary>
    public class RoleUpdateDto
    {
        public long RoleId { get; set; }
        public long SystemId { get; set; }
        public string? RoleName { get; set; }
        public string? Memo { get; set; }
        public bool IsDel { get; set; }
        public long CreateUserId { get; set; }
        public long CreateTime { get; set; }
        public long UpdateUserId { get; set; }
        public long UpdateTime { get; set; }
    }
}
