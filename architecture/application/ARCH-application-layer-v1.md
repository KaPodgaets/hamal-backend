---
id: ARCH-application-layer
title: "Application Layer: Business Logic and Use Cases"
type: component
layer: application
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-16
tags: [application, business-logic, cqrs]
depends_on: [ARCH-domain-entities]
referenced_by: []
---

## Context

The Application Layer contains the core business logic and orchestrates the application's use cases. It is independent of the UI and database technology, making it highly testable and maintainable. It acts as the mediator between the presentation (API) layer and the lower-level domain and infrastructure layers.

## Structure

- **CQRS (Command Query Responsibility Segregation) Pattern**: The logic is separated into Commands (write operations) and Queries (read operations). This simplifies the models and focuses them on a single task.
- **Use Cases / Features**: Logic is organized into feature folders (e.g., `Users`, `Citizens`, `Admin`).
  - **Commands**: `CreateUserCommand`, `UpdateCitizenCommand`, `ImportCitizensCommand`, etc. Each command has a corresponding handler.
  - **Queries**: `GetNextCitizenQuery`, `ExportCitizensQuery`, `GetUserQuery`, etc. Each query has its own handler.
- **Validation**:
  - The `FluentValidation` library is used to define and enforce complex validation rules for incoming commands.
  - Validators are defined within the Application Layer and are automatically executed via a pipeline behavior (e.g., MediatR pipeline). This ensures that no invalid data reaches the core business logic.
- **Interfaces (Abstractions)**: This layer defines interfaces for infrastructure-level concerns, such as repositories and external services.
  - `IUserRepository`, `ICitizenRepository`
  - `IJwtTokenGenerator`, `IFileParser`
  - `IBackgroundJobScheduler`

## Behavior

A typical flow involves a controller from the API layer creating a command or query object and sending it (e.g., via MediatR) to its handler in the Application Layer.

1.  **Command Flow**:
    - A command is received by its handler.
    - The command is first validated. If validation fails, an exception is thrown.
    - The handler uses domain entities and repository interfaces to perform the required business operations.
    - Changes are persisted by calling methods on the repository interfaces.
2.  **Query Flow**:
    - A query is received by its handler.
    - The handler uses repository interfaces to fetch data from the data source, often projecting it directly into a DTO to avoid loading full domain entities.

## Evolution

### Historical

- v1: Initial design using CQRS and FluentValidation as per the project plan.
