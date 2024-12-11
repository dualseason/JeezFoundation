namespace Jeez.Workflow.API.Commons
{
    /// <summary>
    /// 通用结果
    /// </summary>
    public class CommonResult
    {
        /// <summary>
        /// 标识是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string? Message { get; set; }

        public CommonResult()
        {

        }
        public CommonResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    public class CommonResult<T> : CommonResult
    {
        public CommonResult(bool success, string message, T data) : base(success, message)
        {
            Data = data;
        }

        public CommonResult() 
        {
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T? Data { get; set; }
    }

    /// <summary>
    /// 表格数据返回分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonPageResult<T> : CommonResult
    {

        public int PageSize { get; }

        public int Total { get; }

        public int PageCount { get; private set; }

        public List<T> Data { get; }

        public CommonPageResult(int pageSize, int total, List<T> data)
        {
            PageSize = pageSize;
            Total = total;
            Data = data;
            this.Calc();
        }

        private void Calc()
        {
            var result = Total % PageSize;
            if (result == 0)
            {
                PageCount = Total / PageSize;
            }
            else
            {
                PageCount = Total / PageSize + 1;
            }
        }
    }
}
