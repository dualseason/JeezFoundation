---

# JeezFoundation 使用文档

## 1. 项目简介

**JeezFoundation** 是一个提供基础功能和工具的框架，旨在简化开发过程，并为应用程序提供常用的服务和功能模块。它提供了一系列的 API 和库，帮助开发者快速实现业务逻辑。

## 2. 安装和配置

### 2.1 克隆项目

要开始使用 JeezFoundation，首先克隆项目到本地：

```bash
git clone https://github.com/dualseason/JeezFoundation.git
```

### 2.2 安装依赖

确保你已安装所需的依赖项。你可以使用 .NET 的包管理工具来安装所有依赖项：

```bash
cd JeezFoundation
dotnet restore
```

### 2.3 配置

在项目根目录下，你可以找到配置文件，通常是 `appsettings.json` 或类似的配置文件。根据需要修改配置项来适应你的环境。

```json
{
  "AppSettings": {
    "ConnectionString": "your_connection_string_here",
    "LogLevel": "Information"
  }
}
```

## 3. 项目结构

JeezFoundation 项目包含以下主要模块：

- **Core**：核心功能和服务
- **Services**：提供具体的服务实现
- **Utils**：常用工具和扩展
- **Models**：数据模型和实体
- **Controllers**：API 控制器（如果有）

## 4. 快速开始

### 4.1 初始化服务

在 `Startup.cs` 或 `Program.cs` 文件中，初始化 JeezFoundation 提供的服务。你可以将其作为一个依赖注入服务引入到你的项目中。

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 注册 JeezFoundation 服务
        services.AddJeezFoundation();
    }
}
```

### 4.2 使用 API

假设你已成功初始化并配置了相关服务，现在可以使用 JeezFoundation 提供的 API。例如：

```csharp
public class ExampleController : ControllerBase
{
    private readonly IExampleService _exampleService;

    public ExampleController(IExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    [HttpGet("example")]
    public IActionResult GetExampleData()
    {
        var data = _exampleService.GetExampleData();
        return Ok(data);
    }
}
```

### 4.3 配置日志

JeezFoundation 提供了内建的日志系统，你可以在 `appsettings.json` 中配置日志级别：

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

你可以通过以下方式在代码中使用日志：

```csharp
private readonly ILogger<ExampleController> _logger;

public ExampleController(ILogger<ExampleController> logger)
{
    _logger = logger;
}

_logger.LogInformation("This is an information log.");
```

## 5. 高级功能

### 5.1 数据库支持

JeezFoundation 提供了对多种数据库的支持。你可以通过配置 `DbContext` 来连接你的数据库：

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; }
}
```

在 `Startup.cs` 中配置数据库连接：

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
}
```

### 5.2 定时任务

如果项目包含定时任务或调度功能，可以使用相关的工具来配置定时执行的操作。

## 6. 测试

JeezFoundation 项目支持单元测试和集成测试。可以使用 xUnit、NUnit 或 MSTest 来编写测试用例。

```bash
dotnet test
```

## 7. 常见问题

### 7.1 如何处理数据库连接问题？

确保在 `appsettings.json` 中正确配置了数据库连接字符串，并检查数据库是否可用。如果数据库连接失败，可以参考 [连接错误日志](https://docs.microsoft.com/en-us/ef/core/miscellaneous/logging) 来解决问题。

### 7.2 如何更新项目依赖？

运行以下命令来更新所有项目依赖：

```bash
dotnet restore
```

---

