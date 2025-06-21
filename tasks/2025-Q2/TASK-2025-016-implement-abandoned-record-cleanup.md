---
id: TASK-2025-016
title: "Implement Abandoned Record Cleanup Job"
status: done
priority: medium
type: tech_debt
estimate: 8h
created: 2025-06-16
updated: 2025-06-21
parents: [TASK-2025-015]
arch_refs: [ARCH-feature-citizen-workflow, ARCH-infrastructure-layer]
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
      action: "clarified interaction with AppearanceCount logic",
    }
---

## Description

Implemented a background service (`AbandonedCitizenCleanupJob`) that periodically scans for and cleans up abandoned citizen records. This ensures that records don't get permanently locked if an operator's session is interrupted.

## Acceptance Criteria

- A background service was created that runs every 5 minutes.
- The service identifies records with `Status = InProgress` and expired `LockedUntil` timestamps.
- Abandoned records are automatically reset to `Pending` status and returned to the queue.
- The service uses efficient bulk database operations for performance. It does not reset the `AppearanceCount`, so records that have reached the appearance threshold will not be picked up by the `GetNextCitizen` endpoint even after being cleaned up.
- Proper error handling and logging are implemented to prevent service crashes.
