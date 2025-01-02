using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Model;
using Jeez.Workflow.API.Services.interfaces;
using JeezFoundation.Core.Extensions;

namespace Jeez.Workflow.API.Services.implements
{
    public class LogJobs : ILogJobs
    {
        private WorkflowFixtrue WorkflowFixtrue { get; set; }

        private ILogger<LogJobs> Logger { get; set; }

        public LogJobs(WorkflowFixtrue workflowFixtrue, ILogger<LogJobs> logger)
        {
            WorkflowFixtrue = workflowFixtrue;
            Logger = logger;
        }

        public void ExceptionLog(long userId, Exception exception)
        {
            string message = "【系统主动记录】" + exception.ToLogMessage();
            Logger.LogError(message);
        }

        public async void LoginLog(long userId, string userName)
        {
            string message = $"用户名: 【{userName}】, 用户ID：【{userId}】登录成功";
            await WorkflowFixtrue.Db.Log.InsertAsync(new SysLog()
            {
                Application = "Jeez.Workflow.API",
                Logged = DateTime.Now,
                Level = JeezFoundation.Core.Domain.Enum.LogLevel.Login.ToString(),
                Message = message,
                Logger = "System"
            });
        }
    }
}
