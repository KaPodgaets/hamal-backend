---
id: ARCH-feature-citizen-workflow
title: "Feature: Citizen Processing Workflow"
type: feature
layer: application
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-16
tags: [feature, core-logic, workflow, citizen]
depends_on: [ARCH-domain-entities, ARCH-api-layer, ARCH-application-layer]
referenced_by: []
---

## Context

This feature represents the primary business process of the application. It provides a controlled, sequential workflow for call center operators to process citizen records one by one, ensuring that no two operators work on the same record simultaneously.

## Structure

- **API Layer**: `CitizensController` exposes the necessary endpoints.
- **Application Layer**:
  - `GetNextCitizenQuery`: Encapsulates the logic to find and lock the next available citizen record.
  - `UpdateCitizenCommand`: Encapsulates the logic to update a citizen record with new information.
  - `CitizenValidator`: A `FluentValidation` class that enforces strict data integrity rules on the data submitted via the update command.
  - `ReleaseAbandonedCitizensCommand`: A background job command to release records that have been locked for too long.
- **Database**: Uses pessimistic locking (`SELECT ... FOR UPDATE SKIP LOCKED` in PostgreSQL) to handle concurrency safely.

## Behavior

The citizen record lifecycle is `Pending` -> `InProgress` -> `Updated`.

1.  **Get Next Record (`GET /api/citizens/next`)**:
    - An operator requests the next available record.
    - The system executes an atomic database transaction to find the first citizen with `Status = Pending`.
    - It updates the record's `Status` to `InProgress`, sets the `AssignedToUserId` to the operator's ID, and sets `LockedUntil` to 30 minutes in the future.
    - This locked record is then returned to the operator. Due to `SKIP LOCKED`, concurrent requests will not block and will be assigned the next available record.
2.  **Update Record (`PUT /api/citizens/{id}`)**:
    - The operator submits changes for the record they have locked.
    - The system first validates the incoming data against the `CitizenValidator` rules. If invalid, it returns a `400 Bad Request`.
    - If valid, it verifies that the record is still assigned to this operator.
    - The record is updated, and its `Status` is set to `Updated`.
3.  **Abandoned Record Cleanup (Background Job)**:
    - A scheduled job runs periodically (e.g., every 5-10 minutes).
    - It queries for all records where `Status = InProgress` and `LockedUntil` is in the past.
    - For each such record, it resets the `Status` to `Pending` and clears the `AssignedToUserId` and `LockedUntil` fields, returning it to the queue.
