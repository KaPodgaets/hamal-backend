---
id: TASK-2025-021
title: "Phase 2: Callcenter Case API Endpoint Implementation"
status: backlog
priority: high
type: feature
estimate: 8h
created: 2025-06-24
updated: 2025-06-24
parents: [TASK-2025-019]
arch_refs: [ARCH-feature-callcenter-case-workflow, ARCH-api-layer]
audit_log:
  - {
      date: 2025-06-24,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

This task covers creating the public-facing endpoint to allow for `CallcenterCase` creation.

## Acceptance Criteria

1.  **Request DTO:**
    - A new `CreateCallcenterCaseRequest.cs` record is created in `Hamal.Web` with `Id` (int) and `CaseNumber` (string) properties.
2.  **Controller Endpoint:**
    - A new `[HttpPost("106-case")]` endpoint is added to `CitizensController`.
    - The endpoint accepts `CreateCallcenterCaseRequest` from the body.
    - It successfully finds the `CitizenRecord` by ID (or returns `404`).
    - It verifies a case doesn't already exist (or returns `409`).
    - It creates and saves a new `CallcenterCase` record.
    - It returns a `201 Created` or `200 OK` response on success.

## Definition of Done

- The new DTO and API endpoint are implemented and function correctly according to success and error criteria.
