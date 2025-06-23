---
id: ARCH-feature-callcenter-case-workflow
title: "Feature: Call Center Case Workflow"
type: feature
layer: application
owner: "@dev-team"
version: v1
status: planned
created: 2025-06-24
updated: 2025-06-24
tags: [feature, call-center, case-management]
depends_on:
  [ARCH-domain-entities, ARCH-api-layer, ARCH-feature-admin-data-management]
referenced_by: []
---

## Context

This feature introduces a new `CallcenterCase` entity to capture specific, timestamped case data associated with a `CitizenRecord`. It provides an API endpoint for creating these case records and integrates this new data into the existing administrative CSV export.

## Structure

- **Domain Layer (`ARCH-domain-entities`):**

  - A new `CallcenterCase` entity will be created with properties for `Id`, `CallcenterCaseNumber`, `CreatedAt`, `UpdatedAt`, and a foreign key `CitizenRecordId`.
  - The `CitizenRecord` entity will be updated with a one-to-one navigation property to `CallcenterCase`.

- **Infrastructure Layer (`ARCH-infrastructure-layer`):**

  - `AppDbContext`: Will be updated with a `DbSet<CallcenterCase>` and the entity relationship configuration, including a unique index on `CitizenRecordId` to enforce the one-to-one relationship.
  - `CsvExporter`: Will be modified to include the `callcenter_case_number` in the CSV export.

- **API Layer (`ARCH-api-layer`):**
  - A new DTO `CreateCallcenterCaseRequest` will define the request body.
  - `CitizensController`: A new endpoint `POST /api/citizens/106-case` will be added to handle the creation of `CallcenterCase` records.
  - `AdminController`: The `ExportCitizens` method's query will be modified to eagerly load the related `CallcenterCase` data using `.Include()`.

## Behavior

1.  **Case Creation (`POST /api/citizens/106-case`)**:

    - A client sends a request with a `CitizenRecord` ID and a `CaseNumber`.
    - The `CitizensController` finds the specified `CitizenRecord`.
    - It validates that a case does not already exist for this citizen.
    - It creates a new `CallcenterCase` entity, links it to the citizen, and saves it to the database.
    - A `201 Created` response is returned on success.
    - The endpoint will return `404 Not Found` if the citizen doesn't exist and `409 Conflict` if a case already exists.

2.  **Data Export (`GET /api/admin/citizens`)**:
    - The `AdminController` fetches all `CitizenRecord`s, eagerly loading their associated `CallcenterCase` data.
    - The `CsvExporter` generates a CSV file that now includes a `callcenter_case_number` column.
    - For citizens with a case, the column is populated. For those without, it remains empty.

## Evolution

### Planned

- v1: Initial implementation as described.

### Historical

- This is the first version of this feature.
