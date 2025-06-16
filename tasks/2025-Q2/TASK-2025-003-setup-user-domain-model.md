---
id: TASK-2025-003
title: "Setup User Domain Model and Initial Migration"
status: backlog
priority: high
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-16
parents: [TASK-2025-001]
arch_refs: [ARCH-domain-entities, ARCH-infrastructure-layer]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

Define the core `User` entity and `Role` enum within the `Hamal.Domain` project. Set up the Entity Framework Core `AppDbContext` and create the initial database migration to establish the `Users` table in the database.

## Acceptance Criteria

- The `User` entity and `Role` enum are defined in the Domain project.
- `AppDbContext` is created in the Infrastructure project and contains a `DbSet<User>`.
- An EF Core migration for the initial schema has been created.
- The migration can be successfully applied to the PostgreSQL database.
