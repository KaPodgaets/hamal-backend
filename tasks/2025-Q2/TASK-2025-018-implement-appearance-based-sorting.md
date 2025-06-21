--- /dev/null
+++ b/tasks/2025-Q2/TASK-2025-018-implement-appearance-based-sorting.md
@@ -0,0 +1,41 @@
+---
+id: TASK-2025-018
+title: "Implement Appearance-Based Sorting for GetNextCitizen"
+status: done
+priority: high
+type: feature
+estimate: 8h
+assignee: @Robotic-SSE
+created: 2025-06-21
+updated: 2025-06-21
+parents: [TASK-2025-006]
+arch_refs: [ARCH-domain-entities, ARCH-feature-citizen-workflow, ARCH-feature-admin-data-management]
+audit_log:

{date: 2025-06-21, user: "@AI-DocArchitect", action: "created with status done"}

# Description

    Implemented a fairness and efficiency mechanism in the `GET /api/citizens/next` endpoint. This involved adding an `AppearanceCount` property to the `CitizenRecord` to track how many times a record has been presented to an operator. The core logic was updated to prioritize records with the lowest count and to automatically filter out records that are repeatedly skipped.

## Acceptance Criteria

1. **Domain & Database:** The `CitizenRecord` entity has a new `AppearanceCount` integer property. The database schema is updated to include this column with a default value of 0.
2. **`GetNextCitizen` Logic:** The endpoint now only selects records where `AppearanceCount < 3`.
3. **Sorting:** The query for the next citizen now sorts records by `AppearanceCount` (ascending), ensuring less-seen records are served first.
4. **Incrementing:** When a record is successfully fetched and locked, its `AppearanceCount` is incremented by 1 within the same transaction.
5. **Data Import:** Records imported via the CSV upload process are initialized with `AppearanceCount = 0`.
6. **Data Export:** The CSV export includes the `AppearanceCount` column.
7. **Performance:** A filtered index was added to the database on `("AppearanceCount", "Id")` where `StatusInCallCenter = 'Pending' AND "AppearanceCount" < 3` to maintain query performance.

## Definition of Done

- `CitizenRecord` entity was updated.
- `CitizensController` logic for `GetNextCitizen` was refactored.
- `CsvExporter` and `CsvParser` were updated to handle the new field.
- Database migration was created and applied.
- The feature was validated through API testing.

## Notes

- This feature directly addresses the issue of "stuck" records being repeatedly served to operators.
- The filtering threshold of 3 is currently hardcoded in `CitizensController` and can be adjusted in the future if necessary.
- Records that reach the threshold are effectively exhausted from the primary workflow and can be reviewed by administrators via data export.
