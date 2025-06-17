---
id: TASK-2025-007
title: "Setup Citizen Domain Model and Migration"
status: done
priority: high
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-17
parents: [TASK-2025-006]
arch_refs: [ARCH-domain-entities, ARCH-infrastructure-layer]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - { date: 2025-06-17, user: "@AI-DocArchitect", action: "status: backlog -> done" }
---

## Description

Defined the `Citizen` entity with its specified fields and the `CitizenStatus` enum in the `Hamal.Domain` project. The `AppDbContext` was updated to include the `Citizen` entity, allowing EF Core migrations to create the `Citizens` table.

## Acceptance Criteria

- The `Citizen` entity and `CitizenStatus` enum were defined in the `Hamal.Domain` project.
- `AppDbContext` was updated to include a `DbSet<Citizen>`.
- A new EF Core migration for the `Citizens` table was created and can be applied successfully.
