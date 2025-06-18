---
id: TASK-2025-009
title: "Implement Citizen Data Validation Logic"
status: done
priority: high
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-17
parents: [TASK-2025-006]
arch_refs: [ARCH-application-layer, ARCH-feature-citizen-workflow]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
  - { date: 2025-06-17, user: "@AI-DocArchitect", action: "status: backlog -> done" }
---

## Description

Implemented the specified data validation rules for the `UpdateCitizenCommand` using the `FluentValidation` library. This ensures data integrity before any updates are committed to the database.

## Acceptance Criteria

- An `UpdateCitizenCommandValidator` class was created in the `Hamal.Application` project.
- The validator was implemented with rules for `FirstName`, `LastName`, `FamilyNumber`, and conditional rules for `NewStreetName`, `NewBuildingNumber`, and `NewFlatNumber` when `IsAddressWrong` is true.
- The validator is injected and called directly in the `CitizensController` before processing an update.

## Definition of Done

- Validator class `UpdateCitizenCommandValidator` was implemented and integrated.
