using Jeez.Workflow.API.Dtos;

namespace Jeez.Workflow.API.Commons
{
    public static class CommonFunction
    {
        /// <summary>
        /// 创建递归函数。自己遍历自己
        /// 1、找到通用的逻辑代码【规律代码】
        /// 2、从通用的逻辑代码中，找到通用的参数【递归函数的参数】
        /// 3、自己调用自己，实现递归。
        /// </summary>
        public static void GetChildDepts(List<DeptDto> ChildDeptDtos, List<DeptDto> DeptDtos)
        {
            // 3、遍历所有的部门
            foreach (var deptDto in ChildDeptDtos)
            {
                // 3.1、查询子部门
                List<DeptDto> depts = DeptDtos.Where(m => m.ParentId == deptDto.DeptId).ToList();

                // 3.2、赋值子部门
                deptDto.ChildDepts = depts;

                // 3.3、自己调用自己
                GetChildDepts(deptDto.ChildDepts, DeptDtos);
            }
        }
    }
}
