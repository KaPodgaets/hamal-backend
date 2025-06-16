---
id: TASK-2025-008
title: "Implement 'Get Next Citizen' Workflow"
status: backlog
priority: high
type: feature
estimate: 16h
created: 2025-06-16
updated: 2025-06-16
parents: [TASK-2025-006]
arch_refs: [ARCH-feature-citizen-workflow]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

Implement the critical `GET /api/citizens/next` endpoint. This logic must atomically find an available citizen record, lock it for the current operator, and return it. The lock must prevent other operators from receiving the same record.

## Acceptance Criteria

- Calling the endpoint returns a single citizen record with `Status = Pending`.
- Upon being returned, the record's status in the database is changed to `InProgress`.
- The record's `AssignedToUserId` is set to the ID of the calling operator.
- The record's `LockedUntil` field is set to the current time + 30 minutes.
- The operation is atomic and safe under concurrent requests (no two users can get the same record).
- If no records are available, an appropriate response (e.g., `204 No Content`) is returned.
