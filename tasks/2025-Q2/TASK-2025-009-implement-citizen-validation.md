---
id: TASK-2025-009
title: "Implement Citizen Data Validation Logic"
status: backlog
priority: high
type: feature
estimate: 4h
created: 2025-06-16
updated: 2025-06-16
parents: [TASK-2025-006]
arch_refs: [ARCH-application-layer, ARCH-feature-citizen-workflow]
audit_log:
  - {
      date: 2025-06-16,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

Implement the specified data validation rules for the `Citizen` entity using the `FluentValidation` library. This ensures data integrity before any updates are committed to the database.

## Acceptance Criteria

- A `CitizenValidator` class is created in the `Hamal.Application` project.
- The validator implements the following rules:
  - `StreetName`, `NewStreetName`: >2 chars, Hebrew/digits/-.
  - `BuildingNumber`, `NewBuildingNumber`: Digits and max 1 Hebrew letter.
  - `FlatNumber`, `NewFlatNumber`: Integer between 0 and 499.
  - `FirstName`, `LastName`: Hebrew letters only.
  - `FamilyNumber`: Integer > 0.
- The validator is integrated into the application's request pipeline (e.g., via a MediatR behavior).

## Definition of Done

- Validator class is implemented.
- Unit tests are written for the validator, covering both valid and invalid cases for each rule.
