---
id: ARCH-domain-entities
title: "Domain Layer: Entities and Enums"
type: data_model
layer: domain
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-16
tags: [domain, entities, data-model]
depends_on: []
referenced_by: []
---

## Context

The Domain Layer is the core of the application, representing the business concepts, data, and rules. It is completely independent of other layers and contains the enterprise-wide business logic.

## Structure

### Entities

- **`User`**: Represents an operator or administrator in the system.
  - `Id` (Guid, PK)
  - `Username` (string, unique)
  - `PasswordHash` (string)
  - `Role` (enum: `Role`)
- **`Citizen`**: Represents a record to be processed by a call center operator.
  - `Id` (int, PK, Database-Generated Identity)
  - `StreetName` (string)
  - `BuildingNumber` (string)
  - `FlatNumber` (string)
  - `FirstName` (string)
  - `LastName` (string)
  - `FamilyNumber` (int)
  - `IsLonely` (boolean)
  - `IsAddressWrong` (boolean)
  - `NewStreetName` (string, nullable)
  - `NewBuildingNumber` (string, nullable)
  - `NewFlatNumber` (string, nullable)
  - `Status` (enum: `CitizenStatus`)
  - `AssignedToUserId` (Guid, nullable, FK to Users.Id)
  - `LockedUntil` (DateTime?, UTC)
  - `LastUpdatedAt` (DateTime, UTC)

### Enums

- **`Role`**: Defines the access level of a user.
  - `Admin`
  - `User` (Operator)
- **`CitizenStatus`**: Defines the stage of a citizen record in the workflow.
  - `Pending`: New record, available to be taken.
  - `InProgress`: Locked by an operator.
  - `Updated`: Processing is complete.
