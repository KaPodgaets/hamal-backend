---
id: TASK-2025-022
title: "Phase 3: Update CSV Exporter for Callcenter Cases"
status: backlog
priority: medium
type: feature
estimate: 4h
created: 2025-06-24
updated: 2025-06-24
parents: [TASK-2025-019]
arch_refs:
  [ARCH-feature-callcenter-case-workflow, ARCH-feature-admin-data-management]
audit_log:
  - {
      date: 2025-06-24,
      user: "@AI-DocArchitect",
      action: "created with status backlog",
    }
---

## Description

This task covers integrating the new `CallcenterCase` data into the existing administrative CSV export feature.

## Acceptance Criteria

1.  **Data Retrieval:**
    - The `ExportCitizens` method in `AdminController.cs` is modified to eagerly load the related `CallcenterCase` data using `.Include(c => c.CallcenterCase)`.
2.  **CSV Exporter Logic:**
    - The `CsvExporter.cs` implementation is updated.
    - The CSV header now includes `callcenter_case_number`.
    - The value of `citizen.CallcenterCase?.CallcenterCaseNumber` is appended to each record's line in the CSV.

## Definition of Done

- Generating a CSV file via the admin endpoint produces a file with the new column, correctly populated for citizens with and without cases.
