using Microsoft.Extensions.Logging;

namespace LoggerComponent
{
    internal class OrderService
    {
        private readonly ILogger<OrderService> _logger;

        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;
        }

        public void AddOrder()
        {
            try
            {
                _logger.LogInformation("添加订单");
                throw new Exception("添加订单失败");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "添加订单失败");
            }
        }
    }
}
