---
id: TASK-2025-013
title: "Implement Admin Data Export (CSV)"
status: done
priority: medium
type: feature
estimate: 8h
created: 2025-06-16
updated: 2025-06-21
parents: [TASK-2025-011]
arch_refs: [ARCH-feature-admin-data-management]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - {
      date: 2025-06-17,
      user: "@AI-DocArchitect",
      action: "status: backlog -> done",
    }
  - {
      date: 2025-06-21,
      user: "@AI-DocArchitect",
      action: "updated task to include AppearanceCount in export",
    }
---

## Description

Implemented the `GET /api/admin/citizens` endpoint. This allows an administrator to download the entire current state of the `Citizens` table as a comprehensive CSV file, which includes all fields for backup and analysis purposes.

## Acceptance Criteria

- The endpoint was created and is protected for Admin users only.
- The endpoint returns a file download containing all records from the `Citizens` table in CSV format. The export includes all `CitizenRecord` fields like `Fid`, `Phone1`, `IsAnsweredTheCall`, `HasMamad`, `StatusInCallCenter`, `LockedByUserId`, `AppearanceCount`, etc.
- The filename includes a timestamp to prevent browser caching issues.
