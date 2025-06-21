---
id: TASK-2025-007
title: "Setup Citizen Domain Model and Migration"
status: done
priority: high
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-21
parents: [TASK-2025-006]
arch_refs: [ARCH-domain-entities, ARCH-infrastructure-layer]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - {
      date: 2025-06-17,
      user: "@AI-DocArchitect",
      action: "status: backlog -> done",
    }
  - {
      date: 2025-06-21,
      user: "@AI-DocArchitect",
      action: "updated to include AppearanceCount field",
    }
---

## Description

Defined the `CitizenRecord` entity and `CitizenStatus` enum in the `Hamal.Domain` project. The `AppDbContext` was updated to include the `CitizenRecord` entity, allowing EF Core migrations to create the `Citizens` table. The model was later extended to include additional fields for phone numbers and various boolean flags related to call outcomes and citizen status.

## Acceptance Criteria

- The `CitizenRecord` entity and `CitizenStatus` enum were defined in the `Hamal.Domain` project.
- The `CitizenRecord` entity includes fields such as `Fid`, `FirstName`, `LastName`, `Address`, `IsLonely`, `IsAddressWrong`, `Phone1`, `Phone2`, `Phone3`, `IsAnsweredTheCall`, `HasMamad`, `HasMiklatPrati`, `HasMiklatZiburi`, `HasMobilityRestriction`, `AppearanceCount`, `StatusInCallCenter`, `LockedByUserId`, and others.
- `AppDbContext` was updated to include a `DbSet<CitizenRecord>`.
- An EF Core migration for the `Citizens` table was created and applied successfully.
