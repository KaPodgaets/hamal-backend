---
id: TASK-2025-016
title: "Implement Abandoned Record Cleanup Job"
status: backlog
priority: medium
type: tech_debt
estimate: 8h
created: 2025-06-16
updated: 2025-06-16
parents: [TASK-2025-015]
arch_refs: [ARCH-feature-citizen-workflow, ARCH-infrastructure-layer]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

Create a scheduled background job (e.g., using Hangfire or Quartz.NET) that periodically scans for citizen records that have been abandoned. An abandoned record is one with `Status = InProgress` where the `LockedUntil` timestamp has passed. The job should reset these records to make them available again.

## Acceptance Criteria

- A background job is configured to run on a recurring schedule (e.g., every 5-10 minutes).
- The job correctly identifies records where `Status = 'InProgress'` and `LockedUntil < NOW()`.
- For each identified record, the job resets `Status` to `Pending` and clears the `AssignedToUserId` and `LockedUntil` fields.
- The operation is idempotent and handles errors gracefully.
