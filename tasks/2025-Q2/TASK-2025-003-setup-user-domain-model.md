---
id: TASK-2025-003
title: "Setup User Domain Model and Initial Migration"
status: done
priority: high
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-17
parents: [TASK-2025-001]
arch_refs: [ARCH-domain-entities, ARCH-infrastructure-layer]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - {
      date: 2025-06-17,
      user: "@Robotic-SSE",
      action: "completed task by defining entities and configuring DbContext",
    }
  - { date: 2025-06-17, user: "@AI-DocArchitect", action: "status: completed -> done" }
---

## Description

Defined the core `User` entity and `Role` enum within the `Hamal.Domain` project. Set up the Entity Framework Core `AppDbContext` in `Hamal.Infrastructure` to establish the `Users` table in the database.

## Acceptance Criteria

- The `User` entity and `Role` enum were defined in the `Hamal.Domain` project.
- `AppDbContext` was created in the `Hamal.Infrastructure` project and configured with a `DbSet<User>`.
- The EF Core model configuration creates a `Users` table, which can be applied to a PostgreSQL database via migrations.
