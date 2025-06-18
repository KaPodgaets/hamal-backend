---
id: ARCH-feature-admin-data-management
title: "Feature: Admin Bulk Data Management"
type: feature
layer: application
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-17
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
  - `CsvParser`: Implements `IFileParser` for reading citizen data from CSV files.
  - `CsvExporter`: Implements `IFileExporter` for writing citizen data to CSV files.

## Behavior

The system enforces a rigid `export -> clear -> upload` workflow, which is expected to be orchestrated by the client application.

1.  **Export (`GET /api/admin/citizens/export`)**: The administrator first downloads a copy of the current data as a backup or for reference.
2.  **Clear (`DELETE /api/admin/citizens`)**: The administrator clears the entire `Citizens` table. This is a destructive, protected action.
3.  **Upload (`POST /api/admin/citizens/upload`)**: The administrator uploads a new CSV or XLSX file. The system parses this file and performs a bulk insert of the new records into the now-empty table.

This process ensures that the system always contains a complete, consistent dataset from a single source file at any given time, avoiding the complexity of partial updates or merges from file uploads.

## Evolution

### Historical

- v1: Initial implementation providing CSV-based export, clear, and upload functionality.
