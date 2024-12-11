namespace Jeez.Workflow.API.Middlewares
{
    public class HttpGlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpGlobalExceptionMiddleware> _logger;

        public HttpGlobalExceptionMiddleware(RequestDelegate next, ILogger<HttpGlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // 处理异常
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var controllerName = context.GetRouteValue("controller")?.ToString();
            var actionName = context.GetRouteValue("action")?.ToString();
            string errorMsg = $"在请求controller[{controllerName}] 的 action[{actionName}] 时产生异常[{exception.Message}]";

            _logger.LogError(exception, errorMsg);

            context.Response.StatusCode = 500; // 设置HTTP状态码为500
            await context.Response.WriteAsync(errorMsg); // 写入错误信息
        }
    }
}
