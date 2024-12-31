using AutoMapper;
using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Models.SystemDept;
using Jeez.Workflow.API.Services.interfaces;

namespace Jeez.Workflow.API.Services.implements
{
    public class DeptService : IDeptService
    {
        public WorkflowFixtrue WorkflowFixtrue { get; set; }

        public IMapper Mapper { get; set; }

        public DeptService(WorkflowFixtrue workflowFixtrue, IMapper mapper)
        {
            WorkflowFixtrue = workflowFixtrue;
            Mapper = mapper;
        }

        public async Task<CommonResult> DeptCreateAsync(DeptCreateDto deptCreateDto)
        {
            Dept dept = Mapper.Map<Dept>(deptCreateDto);
            using (var tran = WorkflowFixtrue.Db.BeginTransaction())
            {
                try
                {
                    await WorkflowFixtrue.Db.Dept.InsertAsync(dept, tran);
                    SystemDept systemDept = new SystemDept(deptCreateDto.SystemId, dept.DeptId);
                    await WorkflowFixtrue.Db.SystemDept.InsertAsync(systemDept, tran);

                    tran.Commit();
                    return new CommonResult(true, "创建成功");
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return new CommonResult(false, ex.Message);
                } 
            }
        }

        public async Task<CommonResult> DeptDeleteAsync(List<long> deptIds)
        {
            using (var tran = WorkflowFixtrue.Db.BeginTransaction())
            {
                var depts = await WorkflowFixtrue.Db.Dept
                    .FindAllAsync(m => m.IsDel == false && deptIds.Contains(m.DeptId));

                foreach (var dept in depts)
                {
                    dept.IsDel = true;
                    await WorkflowFixtrue.Db.Dept.UpdateAsync(dept, tran);
                }

                tran.Commit();
                return new CommonResult(true, "删除成功");
            }
        }

        public async Task<CommonResult<DeptSelectResultDto>> DeptGetAsync(long deptId)
        {
            Dept dept = await WorkflowFixtrue.Db.Dept.FindAsync(m => m.DeptId == deptId);

            DeptDto deptDto = Mapper.Map<Dept, DeptDto>(dept);

            Dept parentDept = await WorkflowFixtrue.Db.Dept.FindAsync(m => m.IsDel == false && m.DeptId == dept.ParentId);

            var result = new DeptSelectResultDto();
            result.IsDel = deptDto.IsDel;
            result.DeptName = deptDto.DeptName;
            result.DeptCode = deptDto.DeptCode;
            result.DeptId = deptDto.DeptId;
            result.Memo = deptDto.Memo;
            result.SystemId = deptDto.SystemId;
            // 父级部门
            result.ParentDepts = Mapper.Map<DeptDto>(parentDept);

            return new CommonResult<DeptSelectResultDto>(true, "查询成功", result);
        }

        public async Task<CommonResult<List<DeptDto>>> DeptGetListAsync(DeptGetListDto deptGetListDto)
        {
            IEnumerable<Dept> depts = await WorkflowFixtrue.Db.Dept.FindAllAsync(u => u.IsDel == deptGetListDto.IsDel);

            List<DeptDto> deptDtos = Mapper.Map<List<DeptDto>>(depts);

            List<DeptDto> rootDeptDtos = deptDtos.Where(d => d.ParentId == 0).ToList();

            CommonFunction.GetChildDepts(rootDeptDtos, deptDtos);

            return new CommonResult<List<DeptDto>>(true, "查询成功", deptDtos);
        }

        public async Task<CommonPageResult<DeptDto>> DeptGetListPageAsync(DeptGetListPageDto deptGetListPageDto)
        {
            IEnumerable<Dept> depts = await WorkflowFixtrue.Db.Dept
                .FindAllAsync(u => u.IsDel == deptGetListPageDto.IsDel);

            List<DeptDto> deptDtos = Mapper
                .Map<List<DeptDto>>(depts)
                .ToList();

            List<DeptDto> rootDeptDtos = deptDtos
                .Where(d => d.ParentId == 0)
                .ToList();

            CommonFunction.GetChildDepts(rootDeptDtos, deptDtos);

            List<DeptDto> currentDeptDtos = deptDtos
                .Skip((deptGetListPageDto.PageIndex - 1) * deptGetListPageDto.PageSize)
                .Take(deptGetListPageDto.PageSize)
                .ToList();

            return new CommonPageResult<DeptDto>(deptGetListPageDto.PageSize, deptDtos.Count, currentDeptDtos);
        }

        public async Task<CommonResult> DeptUpdateAsync(DeptUpdateDto deptUpdateDto, long deptId)
        {
            Dept dept = await WorkflowFixtrue.Db.Dept
                .FindAsync(m => m.DeptId == deptId);

            if (dept == null)
            {
                throw new CommonException("Dept不存在");
            }

            dept = Mapper.Map<DeptUpdateDto, Dept>(deptUpdateDto);

            await WorkflowFixtrue.Db.Dept.UpdateAsync(dept);

            return new CommonResult(true, "更新成功");
        }
    }
}
