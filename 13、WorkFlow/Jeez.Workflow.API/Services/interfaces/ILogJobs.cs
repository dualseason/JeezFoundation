namespace Jeez.Workflow.API.Services.interfaces
{
    public interface ILogJobs
    {
        /// <summary>
        /// 日志
        /// </summary>
        /// <returns></returns>
        void LoginLog(long userId, string userName);

        /// <summary>
        /// 异常记录
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="exception">异常</param>
        /// <returns></returns>
        void ExceptionLog(long userId, Exception exception);
    }
}
