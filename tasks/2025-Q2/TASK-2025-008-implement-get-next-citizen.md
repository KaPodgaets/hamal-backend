---
id: TASK-2025-008
title: "Implement 'Get Next Citizen' Workflow"
status: done
priority: high
type: feature
estimate: 16h
created: 2025-06-16
updated: 2025-06-17
parents: [TASK-2025-006]
arch_refs: [ARCH-feature-citizen-workflow]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - { date: 2025-06-17, user: "@AI-DocArchitect", action: "status: backlog -> done" }
---

## Description

Implemented the critical `GET /api/citizens/next` endpoint. This logic atomically finds an available citizen record using a `SELECT ... FOR UPDATE SKIP LOCKED` query, locks it for the current operator, and returns it. The lock prevents other operators from receiving the same record.

## Acceptance Criteria

- Calling the endpoint returns a single citizen record that had `Status = Pending`.
- Upon being returned, the record's status in the database was changed to `InProgress`.
- The record's `AssignedToUserId` was set to the ID of the calling operator.
- The record's `LockedUntil` field was set to the current time + 30 minutes.
- The operation is atomic and safe under concurrent requests due to the use of a database transaction and `FOR UPDATE SKIP LOCKED`.
- If no records are available, a `204 No Content` response is returned.
