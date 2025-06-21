---
id: ARCH-feature-citizen-workflow
title: "Feature: Citizen Processing Workflow"
type: feature
layer: application
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-21
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
  - `UpdateCitizenCommand`: Encapsulates the logic to update a citizen record with new information. This includes not just address corrections but also detailed call outcome data (e.g., phone numbers, `IsAnsweredTheCall`) and safety information (e.g., `HasMamad`, `HasMobilityRestriction`, etc.).
  - `UpdateCitizenCommandValidator`: A `FluentValidation` class that enforces strict data integrity rules on the data submitted via the update command.
- **Database**: Uses pessimistic locking (`SELECT ... FOR UPDATE SKIP LOCKED` in PostgreSQL) to handle concurrency safely.
- **Infrastructure Layer**: `AbandonedCitizenCleanupJob` is a background service that releases expired locks.

## Behavior

The citizen record lifecycle is `Pending` -> `InProgress` -> `Updated`.

1.  **Get Next Record (`GET /api/citizens/next`)**:
    - An operator requests the next available record.
    - The system executes an atomic database transaction to find the first citizen where `Status = Pending` and `AppearanceCount <= 3`, ordering first by `AppearanceCount` (ascending) and then by `Id`.
    - If a record is found, its `AppearanceCount` is immediately incremented by one.
    - The system then updates the record's `Status` to `InProgress`, sets the `LockedByUserId` to the operator's ID, and sets `LockedUntil` to 30 minutes in the future.
    - This locked record is then returned to the operator. Due to `SKIP LOCKED`, concurrent requests will not block and will be assigned the next available record.
    - Records that reach an `AppearanceCount` of 3 are effectively removed from this queue, preventing them from being repeatedly assigned.
2.  **Update Record (`PUT /api/citizens/{id}`)**:
    - The operator submits changes for the record they have locked.
    - The system first validates the incoming data against the `UpdateCitizenCommandValidator` rules. If invalid, it returns a `400 Bad Request`.
    - If valid, it verifies that the record is still assigned to this operator and the lock has not expired.
    - The record is updated, and its `Status` is set to `Updated`. All relevant fields from the command, including phone numbers, call status, and safety flags (`HasMamad`, `HasMobilityRestriction`, etc.), are persisted.
3.  **Abandoned Record Cleanup (Background Job)**:
    - A background service (`AbandonedCitizenCleanupJob`) runs periodically (every 5 minutes).
    - It queries for all records where `Status = InProgress` and `LockedUntil` is in the past.
    - For each such record, it resets the `Status` to `Pending` and clears the `LockedByUserId` and `LockedUntil` fields, returning it to the pool of available records. This process does not reset the `AppearanceCount`, so a record with a count of 3 or more will not be picked up by the `Get Next Record` logic even after its lock is cleared.

## Evolution

### Historical

- v1: Initial implementation of the core operator workflow (get next, update) and the abandoned record cleanup job.
