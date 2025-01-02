using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Model;
using Jeez.Workflow.API.Services.interfaces;
using JeezFoundation.Core.Domain.Entities;
using JeezFoundation.Core.Extensions;

namespace Jeez.Workflow.API.Services.implements
{
    public class SystemsService : ISystemsService
    {
        public WorkflowFixtrue WorkflowFixtrue { get; set; }

        public ILogJobs LogJobs { get; set; }

        public SystemsService(WorkflowFixtrue workflowFixtrue, ILogJobs logJobs)
        {
            WorkflowFixtrue = workflowFixtrue;
            LogJobs = logJobs;
        }

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SysSystem> GetByIdAsync(long id)
        {
            return await WorkflowFixtrue.Db.Systems.FindByIdAsync(id);
        }

        /// <summary>
        /// 新增系统
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> InsertAsync(SysSystem system)
        {
            try
            {
                return await WorkflowFixtrue.Db.Systems.InsertAsync(new SysSystem()
                {
                    SystemCode = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now.ToTimeStamp()
                });
            }
            catch(Exception ex)
            {
                LogJobs.ExceptionLog(system.CreateTime, ex);
                return false;
            } 
        }

        /// <summary>
        /// 更新系统数据
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(SysSystem system)
        {
            try
            {
                var dbsystem = await WorkflowFixtrue.Db.Systems.FindByIdAsync(system.SystemId);
                if (dbsystem!= null)
                {
                    dbsystem.SystemName = system.SystemName;
                    dbsystem.IsDel = system.IsDel;
                    dbsystem.Memo = system.Memo;
                    dbsystem.Sort = system.Sort;
                    dbsystem.UpdateTime = DateTime.Now.ToTimeStamp();
                    return await WorkflowFixtrue.Db.Systems.UpdateAsync(dbsystem);
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                LogJobs.ExceptionLog(system.CreateUserId, ex);
                return false;
            }
        }

        /// <summary>
        /// 删除系统数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(List<long> ids, long userid)
        {
            using (var tran = WorkflowFixtrue.Db.BeginTransaction())
            {
                try
                {
                    var dbuserrole = await WorkflowFixtrue.Db.Systems.FindAllAsync(m => m.IsDel == 0 && ids.Contains(m.SystemId));
                    foreach (var item in dbuserrole)
                    {
                        item.IsDel = 1;
                        await WorkflowFixtrue.Db.Systems.UpdateAsync(item);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex) 
                {
                    tran.Rollback();
                    LogJobs.ExceptionLog(userid, ex);
                    return false;
                }
            }
        }
        /// <summary>
        /// 系统列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysSystem>> ListAsync()
        {
            var list = await WorkflowFixtrue.Db.Systems.FindAllAsync(m => m.SystemId > 0);
            return list.ToList();
        }

        /// <summary>
        /// 系统分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Page<SysSystem>> GetPageAsync(int pageIndex, int pageSize)
        {
            var list = await WorkflowFixtrue.Db.Systems.FindAllAsync(m => m.SystemId > 0);
            return new Page<SysSystem>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = list.Count(),
                Items = list.Skip((pageIndex - 1) * pageSize).Take((pageIndex - 1) * pageSize + pageSize)
            };
        }

        /// <summary>
        /// 禁用系统
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DisableSystemAsync(long id)
        {
            try
            {
                var system = await WorkflowFixtrue.Db.Systems.FindAsync(m => m.IsDel == 0 && m.SystemId == id);
                if (system != null)
                {
                    system.IsDel = 1;
                    return await WorkflowFixtrue.Db.Systems.UpdateAsync(system);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogJobs.ExceptionLog(0, ex);
                return false;
            }
        }

        /// <summary>
        /// 启用系统
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> EnableSystemAsync(long id)
        {
            try
            {
                var system = await WorkflowFixtrue.Db.Systems.FindAsync(m => m.IsDel == 1 && m.SystemId == id);
                if (system != null)
                {
                    system.IsDel = 0;
                    return await WorkflowFixtrue.Db.Systems.UpdateAsync(system);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                LogJobs.ExceptionLog(0, e);
                return false;
            }
        }
    }
}
