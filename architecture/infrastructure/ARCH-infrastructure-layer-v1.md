---
id: ARCH-infrastructure-layer
title: "Infrastructure Layer: External Concerns"
type: component
layer: infrastructure
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-16
tags: [infrastructure, persistence, efcore, services]
depends_on: [ARCH-application-layer]
referenced_by: []
---

## Context

The Infrastructure Layer contains implementations for the abstractions (interfaces) defined in the Application Layer. It deals with all external concerns, such as databases, file systems, network communication, and third-party services. This isolates the core application from the details of external technology.

## Structure

- **Persistence**:
  - **Entity Framework Core**: Used as the Object-Relational Mapper (O/RM).
  - **`AppDbContext`**: The EF Core DbContext class that defines the `DbSet`s for entities (`Users`, `Citizens`) and configures their mapping to the database schema.
  - **Repositories**: Concrete implementations of the repository interfaces from the Application Layer (e.g., `UserRepository`, `CitizenRepository`). These classes contain the actual EF Core query logic.
  - **Migrations**: EF Core migrations are used to manage and version the PostgreSQL database schema.
- **Services**:
  - **`JwtTokenGenerator`**: Implements `IJwtTokenGenerator` to create and sign JWTs.
  - **`CsvFileParser`, `XlsxFileParser`**: Implementations of `IFileParser` using libraries like `CsvHelper` and `EPPlus`/`NPOI` to handle bulk data operations.
- **Background Jobs**:
  - An implementation of `IBackgroundJobScheduler` using a library like **Hangfire** or **Quartz.NET**. This is used to schedule and run recurring tasks, such as the `ReleaseAbandonedCitizensCommand`.

## Behavior

Components in this layer are registered with the dependency injection container. When the Application Layer requires an interface (e.g., `ICitizenRepository`), the DI container provides the concrete implementation from this layer. This allows the application's core logic to remain unaware of whether data is being stored in PostgreSQL, SQL Server, or any other database.

## Evolution

### Historical

- v1: Initial design specifying EF Core for persistence and a background job scheduler for system reliability.
