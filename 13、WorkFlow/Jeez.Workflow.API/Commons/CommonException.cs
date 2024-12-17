namespace Jeez.Workflow.API.Commons
{
    public class CommonException : Exception
    {
        public CommonException() { }

        public CommonException(string message) : base(message) { }

        /// <summary>
        /// 异常编号
        /// </summary>
        public string? ErrorNo { get; set; }

        /// <summary>
        /// 抽象Message
        /// </summary>
        public string? ErrorInfo { get; set; }

        /// <summary>
        /// 抽象StackTrace
        /// </summary>
        public string? ErrorReason { get; set; }
    }
}
