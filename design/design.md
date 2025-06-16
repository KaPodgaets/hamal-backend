# Architectural Design Plan V3: Call Center CRM Backend API

## 1. Executive Summary & Goals

This document, the third version of the architectural plan, incorporates specific business rules and data validation logic based on recent clarifications. It finalizes the implementation strategy for a .NET Web API backend for a call center CRM application, detailing the data schema, validation requirements, and precise workflow behaviors.

**Key Goals:**

1.  **Tiered Access Control:** Implement a robust authentication and authorization system distinguishing between 'Admin' and 'User' (Operator) roles, using stateless JWT access tokens.
2.  **Controlled Data Workflow:** Ensure a citizen record can only be processed by one operator at a time, managing its lifecycle with a **30-minute lock time** for `InProgress` records.
3.  **High Data Integrity:** Enforce strict, domain-specific validation rules on all citizen data modifications.

## 2. Current Situation Analysis

The project remains a greenfield application. This revised plan serves as the definitive blueprint for development, incorporating all known requirements for the application's structure, data models, and business logic.

## 3. Proposed Solution / Refactoring Strategy

### 3.1. High-Level Design / Architectural Overview

The plan remains committed to a **Clean Architecture** approach. The four-layer project structure (`Domain`, `Application`, `Infrastructure`, `Api`) is the foundational design principle.

**Dependency Flow Diagram:**

```mermaid
graph TD
    subgraph User Interface
        Api
    end
    subgraph Application
        Application
    end
    subgraph Domain
        Domain
    end
    subgraph Infrastructure
        Infrastructure
    end

    Api -- Depends on --> Application
    Api -- Depends on --> Infrastructure

    Infrastructure -- Implements --> Application
    Infrastructure -- Depends on --> Domain

    Application -- Depends on --> Domain
```

### 3.2. Key Components / Modules

- **Hamal.Domain:**
  - Entities: `User`, `Citizen`.
  - Enums: `Role` (`Admin`, `User`), `CitizenStatus` (`Pending`, `InProgress`, `Updated`).
- **Hamal.Application:**
  - **CQRS Pattern:** Unchanged.
  - **Validation:** Use of `FluentValidation` library to define and apply complex validation rules for commands (e.g., `UpdateCitizenCommand`).
  - **Features/UseCases:**
    - `GetNextCitizenQuery`: Logic to fetch `Pending` citizens and set a 30-minute lock.
    - `UpdateCitizenCommand`: Will now be piped through a validator before execution.
    - `ReleaseAbandonedCitizensCommand`: A background job to reset timed-out records.
  - **Interfaces:** `IUserRepository`, `ICitizenRepository`, `IJwtTokenGenerator`, `IFileParser`, `IBackgroundJobScheduler`.
- **Hamal.Infrastructure:**
  - **Persistence:** `AppDbContext` (EF Core) with the final `Citizen` model.
  - **Services:** `JwtTokenGenerator`, `CsvFileParser`, `XlsxFileParser`.
  - **Background Jobs:** `Hangfire` or `Quartz.NET` integration.
- **Hamal.Api:**
  - Controllers: DTOs will have validation attributes derived from the application layer validators.
  - Middleware: Global error handling, authentication.

### 3.3. Detailed Action Plan / Phases

#### Phase 1: Core Foundation - Authentication & User Management

- **Objective(s):** Establish a secure API foundation with user identity and role-based access control.
- **Priority:** High
- _(Tasks 1.1 - 1.4 remain unchanged.)_

#### Phase 2: Core Feature - Citizen Processing Workflow

- **Objective(s):** Implement the primary workflow for call center operators with specified validation and locking logic.
- **Priority:** High (Depends on Phase 1)
- **Task 2.1: Citizen Domain & Data Model**
  - **Rationale/Goal:** Define the final `Citizen` entity and `CitizenStatus` enum. Create the EF Core migration.
  - **Estimated Effort:** S
  - **Deliverable/Criteria for Completion:** `Citizen` entity in the `Domain` project and a successful database migration.
- **Task 2.2: Implement "Get Next Citizen"**
  - **Rationale/Goal:** Create the logic to atomically find a `Pending` citizen, mark it as `InProgress`, set `LockedUntil` to **now + 30 minutes**, and assign it to the operator.
  - **Estimated Effort:** M
  - **Deliverable/Criteria for Completion:** A `GET /api/citizens/next` endpoint that transactionally fetches and locks a citizen.
- **Task 2.3: Implement Citizen Validation Logic**
  - **Rationale/Goal:** Implement the specified data validation rules for the `Citizen` entity using `FluentValidation`.
  - **Estimated Effort:** S
  - **Deliverable/Criteria for Completion:** A `CitizenValidator` class in the `Application` project that contains all specified field validation rules.
- **Task 2.4: Implement "Update Citizen"**
  - **Rationale/Goal:** Allow an operator to submit updated information for an assigned citizen, ensuring the request passes validation before saving.
  - **Estimated Effort:** M (Depends on 2.3)
  - **Deliverable/Criteria for Completion:** A `PUT /api/citizens/{id}` endpoint that first validates the incoming data using the `CitizenValidator`, then updates the record.

#### Phase 3: Administrative Bulk Data Operations

- **Objective(s):** Provide administrators with a strict, sequential process for managing the citizen dataset.
- **Priority:** Medium (Depends on Phase 2)
- _(Tasks 3.1 - 3.3 remain unchanged.)_

#### Phase 4: System Reliability

- **Objective(s):** Implement automated system maintenance tasks.
- **Priority:** Medium (Depends on Phase 2)
- **Task 4.1: Implement Abandoned Record Cleanup**
  - **Rationale/Goal:** Create a scheduled background job that resets records to `Pending` if they are `InProgress` and their `LockedUntil` timestamp is in the past.
  - **Estimated Effort:** M
  - **Deliverable/Criteria for Completion:** A recurring background job that reliably releases abandoned records based on the 30-minute rule.

### 3.4. Data Model Changes

- **Users Table:** (Unchanged)
  - `Id` (Guid, PK), `Username` (string, unique), `PasswordHash` (string), `Role` (string: "Admin" or "User")
- **Citizens Table:**
  - `Id` (int, PK, Database-Generated Identity), `StreetName` (string), `BuildingNumber` (string), `FlatNumber` (string), `FirstName` (string), `LastName` (string), `FamilyNumber` (int), `IsLonely` (boolean), `IsAddressWrong` (boolean), `NewStreetName` (string, nullable), `NewBuildingNumber` (string, nullable), `NewFlatNumber` (string, nullable), `Status` (string: "Pending", "InProgress", "Updated", indexed), `AssignedToUserId` (Guid, nullable, FK to Users.Id), `LockedUntil` (DateTime?, UTC), `LastUpdatedAt` (DateTime, UTC)
- **Field Validation Rules:**
  - **StreetName, NewStreetName:** Must be >2 chars. Allowed characters: Hebrew letters, digits, '-'.
  - **BuildingNumber, NewBuildingNumber:** Must contain only digits and a maximum of one Hebrew letter.
  - **FlatNumber, NewFlatNumber:** Must be an integer between 0 and 499 (inclusive).
  - **FirstName, LastName:** Must contain only Hebrew letters.
  - **FamilyNumber:** Must be an integer > 0.

### 3.5. API Design / Interface Changes

The API endpoints remain the same. Data Transfer Objects (DTOs) for citizen updates will be validated automatically by the framework, leveraging the `FluentValidation` rules defined in the Application layer.

## 4. Key Considerations & Risk Mitigation

### 4.1. Technical Risks & Challenges

- **Complex Validation:** Implementing custom validation logic for Hebrew characters requires careful regex construction.
  - **Mitigation:** Thoroughly unit test the validator class with a wide range of valid and invalid inputs.
- **Race Conditions in "Get Next Citizen":** Mitigation remains the same: use a database transaction with a pessimistic lock (`SELECT ... FOR UPDATE SKIP LOCKED` in PostgreSQL).
- **Large Data Replacement:** Mitigation remains the same: perform the `clear -> upload` operation within a single transaction.

### 4.2. Dependencies

- **Internal:** Task 2.4 depends on 2.3. Other dependencies are unchanged.
- **External:** Add a dependency on `FluentValidation.AspNetCore`.

### 4.3. Non-Functional Requirements (NFRs) Addressed

- **Data Integrity:** Greatly enhanced by the implementation of specific, server-side validation rules, preventing corrupt or invalid data from entering the system.
- **Reliability:** The 30-minute lock time provides a concrete and predictable mechanism for handling abandoned records, improving system robustness.

## 5. Success Metrics / Validation Criteria

- **Validation Enforcement:** API requests with data violating the specified rules are rejected with a `400 Bad Request` status and a clear error message.
- **Workflow Integrity:** The system correctly locks records for 30 minutes and releases them via the background job if they are abandoned.

## 6. Assumptions Made

- The citizen schema and validation rules provided are now considered complete for the MVP.
- The client application (e.g., a frontend) will be responsible for orchestrating the admin workflow of exporting, clearing, and then uploading.
- Regular expressions for Hebrew character validation will accurately capture the requirements.
