---
id: TASK-2025-014
title: "Implement Admin 'Clear All Data'"
status: done
priority: medium
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-17
parents: [TASK-2025-011]
arch_refs: [ARCH-feature-admin-data-management]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - { date: 2025-06-17, user: "@AI-DocArchitect", action: "status: backlog -> done" }
---

## Description

Implemented the `DELETE /api/admin/citizens` endpoint. This is a destructive action that provides a "reset" function for the citizen data, serving as a required step before a new data upload.

## Acceptance Criteria

- The endpoint was created and is protected for Admin users only.
- Calling this endpoint removes all records from the `Citizens` table using `ExecuteDeleteAsync`.
- The action is atomic.
