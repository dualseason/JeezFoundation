namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户部门关联模型更新Dto
    /// </summary>
    public class UserDeptUpdateDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long DeptId { get; set; }
        public long CreateTime { get; set; }
    }
}
