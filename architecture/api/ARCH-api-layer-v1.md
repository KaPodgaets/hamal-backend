---
id: ARCH-api-layer
title: "API Layer: Presentation and Entry Point"
type: component
layer: presentation
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-17
tags: [api, presentation, aspnetcore]
depends_on: [ARCH-application-layer, ARCH-infrastructure-layer]
referenced_by: []
---

## Context

This layer serves as the entry point for all external requests to the application. It is responsible for handling HTTP communication, routing requests to the appropriate application services, and formatting responses. It is built using ASP.NET Core Web API.

## Structure

- **Controllers**:
  - `AuthController`: Handles user login.
  - `UsersController`: Admin-only endpoints for user CRUD operations.
  - `CitizensController`: Operator-focused endpoints for the main call center workflow (`get next`, `update`).
- **Data Transfer Objects (DTOs)**: Models used to shape the data for API requests and responses. These are validated using rules defined in the Application Layer.
- **Middleware**:
  - **Authentication Middleware**: Validates JWT tokens on incoming requests.
  - **Error Handling**: The default ASP.NET Core exception handling is used to manage unhandled exceptions.
- **Dependency Injection Setup**: Configured in `Program.cs` or `Startup.cs` to wire up services from all layers.

## Behavior

1.  An HTTP request arrives at the Nginx reverse proxy and is forwarded to the API.
2.  ASP.NET Core middleware (authentication, routing) processes the request.
3.  The request is routed to a specific controller action.
4.  The controller validates the incoming DTO and invokes the corresponding command or query in the Application Layer.
5.  Upon receiving a result from the Application Layer, the controller formats it into an HTTP response (e.g., `200 OK` with a JSON body, `201 Created`, `400 Bad Request`) and sends it back to the client.

## Evolution

### Planned

- None at this time.

### Historical

- v1: Initial design based on the greenfield project specification.
