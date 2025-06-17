---
id: TASK-2025-006
title: "Phase 2: Core Feature - Citizen Processing Workflow"
status: done
priority: high
type: feature
estimate: 40h
created: 2025-06-16
updated: 2025-06-17
children: [TASK-2025-007, TASK-2025-008, TASK-2025-009, TASK-2025-010]
arch_refs: [ARCH-feature-citizen-workflow, ARCH-domain-entities]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - { date: 2025-06-17, user: "@AI-DocArchitect", action: "status: backlog -> done" }
---

## Description

This parent task covered the implementation of the primary business workflow for call center operators. It included setting up the citizen data model, implementing the atomic "get next" logic, defining and applying validation rules, and allowing operators to update records.

## Acceptance Criteria

- The `Citizen` entity and `CitizenStatus` enum are defined in the Domain project.
- The `GET /api/citizens/next` endpoint atomically assigns records to operators.
- The `PUT /api/citizens/{id}` endpoint allows operators to update their assigned records.
- Data validation ensures data integrity before updates are committed.
- The system handles concurrent access safely using database-level locking.

## Definition of Done

- All child tasks are completed and their acceptance criteria are met.
