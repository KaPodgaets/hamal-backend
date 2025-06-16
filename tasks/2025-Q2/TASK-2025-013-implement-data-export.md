---
id: TASK-2025-013
title: "Implement Admin Data Export (CSV/XLSX)"
status: backlog
priority: medium
type: feature
estimate: 8h
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

Implement the `GET /api/admin/citizens/export` endpoint. This allows an administrator to download the entire current state of the `Citizens` table as a file.

## Acceptance Criteria

- The endpoint is protected and only accessible to Admin users.
- A `format` query parameter can be used to specify `csv` or `xlsx`.
- The endpoint returns a file download containing all records from the `Citizens` table in the specified format.
