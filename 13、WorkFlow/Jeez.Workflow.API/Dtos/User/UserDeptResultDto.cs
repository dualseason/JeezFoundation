namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 用户部门结果Dto
    /// </summary>
    public class UserDeptResultDto
    {
        public string UserName { set; get; }

        public List<DeptResultDto> deptDtos { get; set; }
    }

    /// <summary>
    /// 部门Dto
    /// </summary>
    public class DeptResultDto
    {
        public long DeptId { set; get; }
        public string DeptName { set; get; }
    }
}
