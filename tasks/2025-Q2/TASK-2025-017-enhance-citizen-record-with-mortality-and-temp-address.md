---
id: TASK-2025-017
title: "Enhance CitizenRecord with Mortality and Temporary Address Fields"
status: backlog
priority: high
type: feature
estimate: 16h
assignee:
created: 2025-06-21
updated: 2025-06-21
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
---

## Description

This task involves enriching the citizen data model to capture mortality status and temporary address information. Five new fields will be added to the `CitizenRecord` entity: `IsDead`, `HasTemporaryAddress`, `TemporaryStreetName`, `TemporaryBuildingNumber`, and `TemporaryFlat`. These changes will propagate through all layers of the application.

## Acceptance Criteria

1.  **Domain & Database:**

    - The `CitizenRecord` entity in `Hamal.Domain` is updated with the five new properties.
    - A database migration is created and applied to add the corresponding columns to the `Citizens` table.

2.  **API & Application Logic:**

    - `CitizenResponse` DTO includes the new fields, so they are returned by `GET /api/citizens/next`.
    - `UpdateCitizenRequest` DTO and `UpdateCitizenCommand` are updated to accept the new fields.
    - The `UpdateCitizen` endpoint in `CitizensController` correctly maps the request and updates the entity in the database.

3.  **Validation:**

    - `UpdateCitizenCommandValidator` is enhanced with conditional validation.
    - When `HasTemporaryAddress` is `true`, the `TemporaryStreetName`, `TemporaryBuildingNumber`, and `TemporaryFlat` fields must not be empty and must conform to the project's existing regex validation patterns.
    - A request with `HasTemporaryAddress: true` and an invalid temporary address field is rejected with a `400 Bad Request`.

4.  **Data Export:**
    - The `CsvExporter` is updated to include the five new fields in the CSV export from `GET /api/admin/citizens`.

## Definition of Done

- Code for all layers (Domain, Infrastructure, Application, Web) is updated.
- Unit and integration tests are updated or created for the new logic.
- The feature is validated via API requests.

## Notes

- This task is based on the refactoring plan submitted on 2025-06-21.
- The data import process (`CsvParser`) should not be changed; these new fields are not expected in the initial data upload.
