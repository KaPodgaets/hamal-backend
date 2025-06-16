---
id: TASK-2025-006
title: "Phase 2: Core Feature - Citizen Processing Workflow"
status: backlog
priority: high
type: feature
estimate: 40h
created: 2025-06-16
updated: 2025-06-16
children: [TASK-2025-007, TASK-2025-008, TASK-2025-009, TASK-2025-010]
arch_refs: [ARCH-feature-citizen-workflow, ARCH-domain-entities]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

This parent task covers the implementation of the primary business workflow for call center operators. It includes setting up the citizen data model, implementing the atomic "get next" logic, defining and applying validation rules, and allowing operators to update records.

## Acceptance Criteria

- The database schema is updated to include the `Citizens` table.
- Operators can request and receive one available citizen record at a time, which becomes locked.
- Operators can submit updates for records assigned to them.
- Submitted data is validated against a specific set of rules.

## Definition of Done

- All child tasks are completed and their acceptance criteria are met.
