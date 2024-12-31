namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 部门模型创建Dto
    /// </summary>
    public class DeptCreateDto
    {
        /// <summary>
        /// 属于那个系统，必填
        /// </summary>
        public long SystemId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public required string DeptName { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        public required string DeptCode { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 部门路径
        /// </summary>
        public string? Path { get; set; }
    }
}
