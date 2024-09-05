using Karambolo.Extensions.Logging.File;
using LoggerComponent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var connectionString = "server=localhost;database=automail;user=root;password=1234;";

ServiceCollection services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Trace);
    builder.AddFile(options =>
    {
        options.RootPath = AppContext.BaseDirectory;
        options.Files = [new LogFileOptions { Path = "logs/log.txt" }];
    });
});
services.AddLogging();
services.AddSingleton<OrderService>();

var provider = services.BuildServiceProvider();
OrderService orderService = provider.GetService<OrderService>();
orderService.AddOrder();

Console.ReadKey();