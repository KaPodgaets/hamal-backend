---
id: ARCH-feature-admin-data-management
title: "Feature: Admin Bulk Data Management"
type: feature
layer: application
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-21
tags: [feature, admin, data-management, bulk-operations]
depends_on: [ARCH-domain-entities, ARCH-api-layer, ARCH-infrastructure-layer]
referenced_by: []
---

## Context

This feature provides administrators with tools to manage the entire citizen dataset in bulk. It follows a strict, procedural workflow to ensure data integrity and prevent accidental data corruption.

## Structure

- **API Layer**: `AdminController` exposes endpoints for these operations.
- **Application Layer (Interfaces)**: Defines `IFileParser` and `IFileExporter` to abstract file operations.
- **Infrastructure Layer (Implementations)**:
  - `CsvParser`: Implements `IFileParser` for reading citizen data from CSV files. When importing, it sets a default `AppearanceCount` of 0 for new records.
  - `CsvExporter`: Implements `IFileExporter` for writing citizen data to CSV files. The export is comprehensive, including all fields from the `CitizenRecord` entity (including `AppearanceCount`) for complete data backup and analysis.

## Behavior

The system enforces a rigid `export -> clear -> upload` workflow, which is expected to be orchestrated by the client application.

1.  **Export (`GET /api/admin/citizens`)**: The administrator first downloads a copy of the current data as a backup or for reference.
2.  **Clear (`DELETE /api/admin/citizens`)**: The administrator clears the entire `Citizens` table.
3.  **Upload (`POST /api/admin/citizens`)**: The administrator uploads a new CSV file. The system parses this file and performs a bulk insert of the new records, which are initialized with an `AppearanceCount` of 0.

This process ensures the system contains a consistent dataset from a single source file, avoiding the complexity of partial updates from file uploads.

## Evolution

### Historical

- v1: Initial implementation providing CSV-based export, clear, and upload functionality.
