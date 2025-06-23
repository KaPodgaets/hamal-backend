---
id: TASK-2025-020
title: "Phase 1: Callcenter Case Domain & Data Model"
status: backlog
priority: high
type: feature
estimate: 4h
created: 2025-06-24
updated: 2025-06-24
parents: [TASK-2025-019]
arch_refs: [ARCH-feature-callcenter-case-workflow, ARCH-domain-entities]
audit_log:
  - {
      date: 2025-06-24,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

This task covers establishing the core domain entity (`CallcenterCase`) and preparing the database schema for the new feature.

## Acceptance Criteria

1.  **`CallcenterCase` Entity:**
    - A `CallcenterCase.cs` class is created in `Hamal.Domain`.
    - It contains properties: `Id` (int), `CallcenterCaseNumber` (string), `CitizenRecordId` (int), `CreatedAt` (DateTime), `UpdatedAt` (DateTime?).
2.  **`CitizenRecord` Entity:**
    - The `CitizenRecord.cs` class in `Hamal.Domain` is updated with a nullable navigation property: `public CallcenterCase? CallcenterCase { get; set; }`.
3.  **`AppDbContext` Update:**
    - A `DbSet<CallcenterCase>` is added to `AppDbContext.cs`.
    - The `OnModelCreating` method is configured with a one-to-one relationship between `CitizenRecord` and `CallcenterCase`, ensuring `CitizenRecordId` in `CallcenterCase` is a foreign key with a unique index.
4.  **Database Migration:**
    - An EF Core migration is generated that correctly creates the `CallcenterCases` table with appropriate columns, keys, and constraints.
    - The migration can be successfully applied to the database.

## Definition of Done

- All code changes are complete and a valid database migration has been created.
