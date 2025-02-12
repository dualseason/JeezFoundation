# JeezFoundation 使用文档

# JeezFoundation 项目文档

## 项目简介
JeezFoundation 是一个基于 .NET 的模块化基础框架，包含多个子项目，涵盖了 Consul、Dapper、Redis、WeChat、WorkFlow 等功能模块。该项目旨在为开发者提供一个可扩展、模块化的基础框架，以便快速构建企业级应用。

## 目录
- [项目简介](#项目简介)
- [项目结构](#项目结构)
- [安装指南](#安装指南)
- [使用说明](#使用说明)
- [依赖项](#依赖项)
- [贡献指南](#贡献指南)
- [许可证](#许可证)

## 项目结构
JeezFoundation 项目包含以下子项目：

- **JeezFoundation.Core**: 核心模块，提供基础功能和工具类。
- **JeezFoundation.Consul**: 提供 Consul 服务发现和配置管理的集成。
- **JeezFoundation.Dapper**: 提供 Dapper ORM 的扩展和工具类。
- **JeezFoundation.Redis**: 提供 Redis 缓存的集成和管理。
- **JeezFoundation.WeChat**: 提供微信相关功能的集成，如消息处理、事件处理等。
- **JeezFoundation.WorkFlow**: 提供工作流引擎的实现，支持会签、流程节点扩展等功能。
- **JeezFoundation.Horoscope**: 提供八字排盘、童限计算等功能。
- **JeezFoundation.Test**: 提供单元测试和集成测试的支持。
- **JeezFoundation.Tool**: 提供一些通用的工具类和扩展方法。

### 项目文件结构



## 安装指南
### 环境要求
- 操作系统：Windows / macOS / Linux
- .NET SDK：8.0
- 其他依赖项：Consul、Redis、Dapper 等

### 安装步骤
1. 克隆仓库：
   ```bash
   git clone https://github.com/dualseason/JeezFoundation.git
   ```
2. 进入项目目录：
```bash
cd JeezFoundation
```
3. 恢复 NuGet 包：
```bash
dotnet restore
```

使用说明
JeezFoundation.Consul
Consul 模块提供了服务发现和配置管理的功能。你可以通过以下方式使用：

```bash
var serviceDiscoveryOptions = new ServiceDiscoveryOptions
{
    Service = new ServiceOptions(),
    Consul = new ConsulOptions()
};
```
JeezFoundation.WorkFlow
工作流模块提供了流程节点扩展和会签功能。你可以通过以下方式使用：

```bash
var chatData = new ChatData
{
    ChatType = ChatType.Parallel,
    ParallelCalcType = ChatParallelCalcType.OneHundredPercent
};
```

JeezFoundation.WeChat
微信模块提供了微信消息和事件的处理功能。你可以通过以下方式使用：
```bash
var shortVideoMsg = new RequestShortVideoMsg
{
    MediaId = "media_id",
    ThumbMediaId = "thumb_media_id"
};
```

依赖项
以下是项目的主要依赖项：

```bash
Consul: 1.7.14.6
Dapper: 2.1.35
Redis: 2.8.22
Newtonsoft.Json: 13.0.3
System.ComponentModel.Annotations: 4.5.0
```
贡献指南
Fork 项目仓库。
创建新的分支：
```bash
git checkout -b feature/your-feature
```
提交你的更改：
```bash
git commit -m "Add some feature"
```
推送到分支：
```bash
git push origin feature/your-feature
```
提交 Pull Request。
许可证
本项目采用 MIT 许可证。

### 说明
- **项目结构**：根据你提供的代码文件，列出了主要子项目和文件结构。
- **安装指南**：提供了基本的安装和编译步骤。
- **使用说明**：简要介绍了如何使用 Consul、WorkFlow 和 WeChat 模块。
- **依赖项**：列出了项目的主要依赖项。
- **贡献指南**：提供了如何为项目做出贡献的步骤。

你可以根据项目的实际情况进一步扩展和修改这个文档。
