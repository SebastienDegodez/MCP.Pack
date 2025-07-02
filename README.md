# MCP.Pack 🚀

A modern .NET server for Model Context Protocol (MCP) and NuGet package management 📦.
Built with Clean Architecture 🏗️ and Domain-Driven Design (DDD) 🧠, supporting AOT compilation ⚡ and comprehensive automated testing 🧪.

---

## ✨ Features
- 🔍 Discover and search NuGet packages via the Model Context Protocol (MCP)
- 📑 Retrieve rich package metadata for any NuGet feed
- 🧩 Designed for extensibility and integration in modern .NET ecosystems

## 🛠️ Technical Highlights
- 🏗️ **Clean Architecture**: strict separation of Domain, Application, Infrastructure, and API layers
- ⚡ **AOT (Ahead-Of-Time) compilation**: fast startup, low memory usage
- 🧪 **Comprehensive automated tests**: unit, integration, and architecture validation
- 💎 **Modern .NET 9**: minimal APIs, dependency injection, async/await everywhere

## 🗂️ Project Structure

```
McpPack.sln
src/
  McpPack.Domain/         # Domain logic, aggregates, value objects, repositories
  McpPack.Application/    # Use cases, service interfaces
  McpPack.Infrastructure/ # Implementations for repositories, external services
  McpPack.Api/            # API endpoints, orchestration

tests/
  McpPack.UnitTests/      # Unit tests for Domain & Application
  McpPack.IntegrationTests/ # Integration & architecture tests
```

## 🚀 Getting Started

### 🧰 Prerequisites
- 🟣 [.NET 9 SDK](https://dotnet.microsoft.com/download)
- 📊 (Optional) [dotnet-reportgenerator-globaltool](https://github.com/danielpalme/ReportGenerator) for coverage reports

### 🏃‍♂️ Build & Run
```sh
dotnet build
dotnet run --project src/McpPack.Api
```

### 🧪 Test
```sh
dotnet test
```

## 🏛️ Architecture Principles
- 🧠 **Domain**: no dependencies, pure business logic
- 🎯 **Application**: orchestrates use cases, depends only on Domain
- 🏗️ **Infrastructure**: implements interfaces, depends on Application & Domain
- 🌐 **API**: exposes endpoints, depends only on Infrastructure
- 🧪 **Tests**: separated by type (unit/integration/architecture)

## 🤝 Contributing
- 📝 Follow [Conventional Commits](https://www.conventionalcommits.org/)
- 🧪 Write tests for all new features and bugfixes
- 🧹 Keep code clean and robust

## 📄 License
MIT

---

> Because good enough is never enough. 😎
