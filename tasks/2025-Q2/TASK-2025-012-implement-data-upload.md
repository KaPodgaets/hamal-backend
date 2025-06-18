---
id: TASK-2025-012
title: "Implement Admin Data Upload (CSV)"
status: done
priority: medium
type: feature
estimate: 8h
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

Implemented the `POST /api/admin/citizens/upload` endpoint. This allows an administrator to upload a CSV file to populate the `Citizens` table. This operation is intended to be performed on an empty table as part of the data replacement workflow.

## Acceptance Criteria

- The endpoint was created and accepts `multipart/form-data` file uploads.
- The endpoint is protected and only accessible to Admin users.
- The service correctly parses the uploaded CSV file and performs a bulk insert of the records into the database.
- The system returns a `409 Conflict` if the `Citizens` table is not empty before upload.
- Malformed CSV files or rows result in a descriptive `400 Bad Request` error response.
