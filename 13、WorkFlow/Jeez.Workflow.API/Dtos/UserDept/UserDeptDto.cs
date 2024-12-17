namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户部门关联模型集合查询结果Dto
    /// </summary>
    public class UserDeptDto
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public long DeptId { get; set; }
        public List<UserDeptList>? UserDeptLists { get; set; }
    }

    /// <summary>
    /// 用户部门集合
    /// </summary>
    public class UserDeptList
    {
        public long DeptId { get; set; }
        public string? DeptName { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected { set; get; } 
    }
}
