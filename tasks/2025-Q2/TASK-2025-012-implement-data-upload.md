---
id: TASK-2025-012
title: "Implement Admin Data Upload (CSV/XLSX)"
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

Implement the `POST /api/admin/citizens/upload` endpoint. This allows an administrator to upload a file (CSV or XLSX) to populate the `Citizens` table. This operation is intended to be performed on an empty table as part of the data replacement workflow.

## Acceptance Criteria

- The endpoint accepts `multipart/form-data` file uploads.
- The endpoint is protected and only accessible to Admin users.
- The service correctly parses the uploaded file and performs a bulk insert of the records into the database.
- Malformed files or rows result in a descriptive error response.
