namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户模型创建Dto
    /// </summary>
    public class UserCreateDto
    {
        public long UserId { get; set; }
        public long SystemId { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; }
        public string JobNumber { get; set; }
        public string Password { get; set; }
        public string HeadImg { get; set; }
        public bool IsDel { get; set; }
        public long CreateUserId { get; set; }
        public long CreateTime { get; set; }
        public long UpdateUserId { get; set; }
        public long UpdateTime { get; set; }}
}
