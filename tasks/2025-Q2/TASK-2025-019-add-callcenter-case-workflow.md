---
id: TASK-2025-019
title: "Feature: Add CallcenterCase Entity and Workflow"
status: done
priority: high
type: feature
estimate: 24h
created: 2025-06-24
updated: 2025-06-24
children:
  - TASK-2025-020
  - TASK-2025-021
  - TASK-2025-022
arch_refs: [ARCH-feature-callcenter-case-workflow]
audit_log:
  - {
      date: 2025-06-24,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - {
      date: 2025-06-24,
      user: "@AI-DocArchitect",
      action: "status: backlog -> done; all child tasks completed",
    }
---

## Description

This epic covers the work required to introduce a new `CallcenterCase` entity into the system. This involves creating the database table, adding an API endpoint for case creation, and updating the CSV export to include the new data.

## Acceptance Criteria

- A `CallcenterCase` can be associated with a `CitizenRecord` via a new API endpoint.
- The new case data is included in the administrative CSV export.
- The implementation follows the architectural decisions outlined in `ARCH-feature-callcenter-case-workflow`.

## Definition of Done

- All child tasks (Phases 1-3) are completed and their acceptance criteria are met.
