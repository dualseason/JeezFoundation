namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户部门关联模型创建Dto
    /// </summary>
    public class UserDeptCreateDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long DeptId { get; set; }
        public long CreateTime { get; set; }
    }
}
