namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户模型集合查询结果Dto
    /// </summary>
    public class UserDto
    {
        public long UserId { get; set; }
        public long DepartId { get; set; }
        public string? Account { get; set; }
        public string? UserName { get; set; }
        public string? JobNumber { get; set; }
        public string? Password { get; set; }
        public string? HeadImg { get; set; }
    }
}
