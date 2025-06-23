---
id: TASK-2025-017
title: "Enhance CitizenRecord with Mortality and Temporary Address Fields"
status: done
priority: high
type: feature
estimate: 8h
assignee: @Robotic-SSE
created: 2025-06-21
updated: 2025-06-24
parents: [TASK-2025-006]
arch_refs:
  [
    ARCH-domain-entities,
    ARCH-feature-citizen-workflow,
    ARCH-api-layer,
    ARCH-feature-admin-data-management,
  ]
audit_log:
  - {
      date: 2025-06-21,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - { date: 2025-06-24, user: "@AI-DocArchitect", action: "status: backlog -> done; updated to reflect completed implementation" }
---

## Description

The citizen data model was enriched to capture mortality status and temporary address information. Nine new fields were added to the `CitizenRecord` entity: `IsDead`, `IsLeftTheCity`, `HasTemporaryAddress`, `IsTemporaryAbroad`, `TemporaryStreetName`, `TemporaryBuildingNumber`, `TemporaryFlat`, `Phone1`, `Phone2`, and `Phone3`. These changes were propagated through all layers of the application.

## Acceptance Criteria

1.  **Domain & Database:**

    - The `CitizenRecord` entity in `Hamal.Domain` was updated with the new properties.
    - A database migration was created and applied to add the corresponding columns to the `Citizens` table.

2.  **API & Application Logic:**

    - `CitizenResponse`, `UpdateCitizenRequest`, and `UpdateCitizenCommand` were updated to include the new fields.
    - The `UpdateCitizen` endpoint in `CitizensController` was updated to correctly map the request and persist the new data.

3.  **Validation:**

    - `UpdateCitizenCommandValidator` was enhanced with conditional validation for the temporary address fields.
    - When `HasTemporaryAddress` is `true` and `IsTemporaryAbroad` is `false`, the temporary address fields are required and validated.
    - An invalid request is correctly rejected with a `400 Bad Request`.

4.  **Data Export:**
    - The `CsvExporter` was updated to include the new fields in the CSV export from `GET /api/admin/citizens`.

## Definition of Done

- Code for all layers (Domain, Infrastructure, Application, Web) is updated.
- The feature was validated via API requests.
- The data import process (`CsvParser`) was not changed; these new fields are not part of the initial data upload schema.

## Notes

- This task is based on the refactoring plan submitted on 2025-06-21.
- The data import process (`CsvParser`) should not be changed; these new fields are not expected in the initial data upload.
