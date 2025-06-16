---
id: TASK-2025-014
title: "Implement Admin 'Clear All Data'"
status: backlog
priority: medium
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-16
parents: [TASK-2025-011]
arch_refs: [ARCH-feature-admin-data-management]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

Implement the `DELETE /api/admin/citizens` endpoint. This is a destructive action that provides a "reset" function for the citizen data, serving as a required step before a new data upload.

## Acceptance Criteria

- The endpoint is protected and only accessible to Admin users.
- Calling this endpoint removes all records from the `Citizens` table.
- The action is performed within a transaction.
