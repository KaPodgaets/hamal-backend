---
id: TASK-2025-010
title: "Implement 'Update Citizen' Endpoint"
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

Implement the `PUT /api/citizens/{id}` endpoint to allow an operator to submit updated information for a citizen record that is assigned to them. The submitted data must pass validation before being saved.

## Acceptance Criteria

- The endpoint is protected and requires an authenticated user.
- A request to update a citizen record first triggers the `CitizenValidator`. Invalid requests are rejected with a `400 Bad Request`.
- An operator can only update a record that is currently assigned to them (`AssignedToUserId` matches the operator's ID).
- Upon successful update, the citizen's `Status` is changed to `Updated`.
- The `LastUpdatedAt` field is updated with the current timestamp.
