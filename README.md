# McpPack MCP Server (.NET)

This project is a Model Context Protocol (MCP) server built with .NET, following Clean Architecture and Domain-Driven Design (DDD) principles.

## Structure
- **McpPack.Domain**: Domain logic, aggregates, entities, value objects, repositories, domain events
- **McpPack.Application**: Application services, use cases, interfaces
- **McpPack.Infrastructure**: Infrastructure implementations (e.g., data access, external services)
- **McpPack.Api**: ASP.NET Core Web API (entry point)

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Build
```sh
dotnet build
```

### Run
```sh
dotnet run --project McpPack.Api
```

### Debug
- Open the solution in VS Code or Visual Studio
- Set breakpoints in any project
- Start debugging (F5)

## Notes
- Follow DDD and Clean Architecture best practices for all new code.
- See `.github/copilot-instructions.md` for Copilot guidance.
