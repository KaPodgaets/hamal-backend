---
id: TASK-2025-007
title: "Setup Citizen Domain Model and Migration"
status: backlog
priority: high
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-16
parents: [TASK-2025-006]
arch_refs: [ARCH-domain-entities, ARCH-infrastructure-layer]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

Define the `Citizen` entity with its specified fields and the `CitizenStatus` enum in the `Hamal.Domain` project. Create and apply an EF Core migration to add the `Citizens` table to the database.

## Acceptance Criteria

- The `Citizen` entity and `CitizenStatus` enum are defined in the Domain project.
- `AppDbContext` is updated to include a `DbSet<Citizen>`.
- A new EF Core migration for the `Citizens` table is created and can be applied successfully.
