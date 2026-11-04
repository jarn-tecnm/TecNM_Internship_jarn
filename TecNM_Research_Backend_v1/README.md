# TecNM Research Backend
## Tecnológico Nacional de México — Campus Monclova

Backend API and database for managing institutional research projects, researcher profiles, committee reviews, academic authorization, progress reports, and research outputs.

> This README defines the intended scope and proposed technical organization. The repository and database have not been inspected as part of this adaptation; modules, tables, configuration keys, scripts, and workflows described below must be implemented or verified against the actual code. Checklist items remain open until their acceptance criteria are met.

## Table of Contents

1. [Purpose and scope](#purpose-and-scope)
2. [Technology and architecture](#technology-and-architecture)
3. [Roles and permissions](#roles-and-permissions)
4. [Project workflow](#project-workflow)
5. [Database design](#database-design)
6. [API responsibilities](#api-responsibilities)
7. [Documents, evidence, and exports](#documents-evidence-and-exports)
8. [Local development](#local-development)
9. [Database change management](#database-change-management)
10. [Production deployment](#production-deployment)
11. [Testing](#testing)
12. [Development checklist](#development-checklist)

## Purpose and Scope

The application will support the research project lifecycle at TecNM Campus Monclova.

Researchers will be able to:

- Maintain their basic CV, including academic background, institutional affiliation, research interests, and relevant professional or research experience.
- Register research projects with a research protocol, objectives, work plan, schedule, budget, and funding sources.
- Identify the principal investigator responsible for each project and register internal collaborators, external collaborators, and supporting students.
- Submit proposals for committee review and respond to requests for corrections.
- Track subsequent authorization by the Academic Subdirectorate (Subdirección Académica).
- Submit progress reports, supporting evidence, and resulting research outputs.
- Download institutional forms and attach completed documents, such as a rights assignment letter (carta de cesión de derechos).

The system will also allow authorized users to download Excel and PDF reports and access the supporting evidence within their permission scope. Administrators will manage research lines, calls for proposals, reference data, users, and institutional document templates.

## Technology and Architecture

The proposed technical baseline follows the supplied README: ASP.NET Core on .NET 8, Entity Framework Core, and SQL Server. Confirm the target framework and package versions in the actual project before implementation. Node.js is not required to build or run the backend.

The backend will use a feature-based organization inspired by Vertical Slice architecture. Each feature groups its endpoints, request/response DTOs, validation, and application services. Controllers handle HTTP concerns; services implement business rules and coordinate database and file operations. Keeping this separation makes the business rules easier to test independently.

### Proposed source organization

| Location | Responsibility |
|---|---|
| `Data/` | `AppDbContext`, EF Core mappings, and persistence configuration. |
| `Entities/` | Database entities; one file per entity. |
| `Features/Auth.cs` | Authentication, account activation, tokens, and password management. |
| `Features/Users.cs` | User accounts, role assignments, and access administration. |
| `Features/Researchers.cs` | Researcher profiles and basic CV information. |
| `Features/Students.cs` | Student records and project participation. |
| `Features/Projects/Projects.cs` | Project registration, protocol, objectives, ownership, and submissions. |
| `Features/Projects/WorkPlans.cs` | Activities, milestones, deadlines, and assigned responsibilities. |
| `Features/Projects/Participants.cs` | Principal investigator, internal/external collaborators, and supporting students. |
| `Features/Projects/Budgets.cs` | Budget items and funding sources. |
| `Features/Reviews.cs` | Committee assignments, individual reviews, observations, and committee decisions. |
| `Features/Authorizations.cs` | Academic Subdirectorate decisions. |
| `Features/ProgressReports.cs` | Progress reporting and links to planned activities. |
| `Features/ResearchOutputs.cs` | Resulting publications, presentations, prototypes, and other output types. |
| `Features/Documents.cs` | Attachment metadata, private storage, and authorized downloads. |
| `Features/Templates.cs` | Institutional forms, template versions, and required attachments. |
| `Features/Reports.cs` | Permission-scoped queries, Excel/PDF exports, and evidence downloads. |
| `Features/ReferenceData/` | Research lines, calls for proposals, academic programs, and other reference data. |
| `Features/Shared/` | Common API responses, authorization support, auditing, email, and error handling. |
| `EmailTemplates/` | Account and workflow notification templates. |
| `Program.cs` | Configuration, dependency registration, authentication, and the HTTP pipeline. |
| `database/` | Versioned SQL scripts for schema changes and initial reference data. |
| `tests/` | Unit and integration test projects. |

Simple features may stay in a single file; larger ones can be split within their feature folder. Keep feature-specific types inside that feature and place only reusable components in `Shared`.

## Roles and Permissions

Role names below are proposed application identifiers. Permissions must also consider project ownership, review assignments, academic program, and explicit access grants.

| Role | Intended permissions |
|---|---|
| `Researcher` | Maintain their own profile/CV; manage projects for which they are responsible; submit proposals, corrections, progress reports, and outputs; access permitted documents and exports. |
| `CommitteePresident` | Review assigned projects and record the president's assessment; coordinate the committee decision according to the approved review rules. |
| `CommitteeSecretary` | Review assigned projects and record the secretary's assessment and committee documentation. |
| `ProgramHead` | Act as Jefe de Carrera and review assigned projects within the applicable academic scope. |
| `AcademicSubdirectorate` | Review committee-endorsed submissions and record the subsequent institutional authorization decision. |
| `AuthorizedViewer` | View records and download reports or evidence explicitly allowed by their access grants. |
| `Administrator` | Manage accounts, permissions, research lines, calls, reference data, and institutional templates. |

Administrative access must not automatically count as committee endorsement or academic authorization. External collaborators and students are project participants; they do not automatically receive user accounts or access. A user may hold multiple roles, subject to conflict-of-interest rules that must be agreed before implementation.

Enforce authorization on every API action, including individual document downloads and exports. Hiding actions in the frontend is insufficient.

## Project Workflow

The required sequence is researcher submission → committee review → Academic Subdirectorate authorization → progress and output reporting.

The following states are proposed for implementation:

| State | Meaning and allowed next action |
|---|---|
| `Draft` | The responsible researcher edits the proposal and attachments. |
| `UnderCommitteeReview` | A submitted version is being reviewed by the assigned president, secretary, and program head. |
| `ChangesRequested` | The researcher addresses documented observations and submits a new version to the requesting stage. |
| `CommitteeEndorsed` | The committee decision is complete; the submission can proceed to academic authorization. |
| `PendingAcademicAuthorization` | The Academic Subdirectorate reviews the committee-endorsed submission. |
| `Authorized` | The project may enter progress and output reporting. |
| `InProgress` | The researcher records progress, evidence, and outputs. |
| `Completed` | Project closure requirements have been met under the institution's agreed rules. |
| `Rejected` | A rejection decision and its reason are retained. |
| `Cancelled` | An authorized cancellation and its reason are retained. |

Each transition must be validated by the backend and saved with the actor, timestamp, reason, and submission version. Submitted versions must remain available as reviewed; corrections create a new version rather than overwriting the record reviewed by the committee.

Before coding the review engine, confirm whether committee endorsement requires unanimity, majority, or another rule; whether reviews are sequential or parallel; and who records the final committee decision. Also confirm correction routing, deadlines, conflict-of-interest handling, cancellation, and project closure rules. These are pending business decisions, not established institutional policy.

## Database Design

Use SQL Server as the relational source of truth. The proposed database name is `TecNMResearchDB`; confirm the actual deployment name before creating or updating it.

### Proposed entity groups

| Area | Proposed tables | Main relationships |
|---|---|---|
| Identity and access | `Users`, `Roles`, `UserRoles`, `AccessGrants` | Users can hold several roles; grants define resource scope and permitted actions. |
| Researcher CV | `Researchers`, `ResearcherDegrees`, `ResearcherExperience` | A researcher may link to a user account and have multiple CV entries. |
| Institutional reference data | `AcademicPrograms`, `ResearchLines`, `CallsForProposals`, `OutputTypes` | Calls define submission periods and requirements; programs and lines classify projects and researchers. |
| Projects | `Projects`, `ProjectResearchLines`, `ProjectObjectives` | Each project has one responsible researcher, objectives, research lines, and a call association when applicable. |
| Proposal versions | `ProjectSubmissions` | Each submission stores an immutable snapshot of the protocol, objectives, work plan, participants, budget, funding, and attachment versions presented for review. |
| Participants | `ProjectCollaborators`, `Students`, `ProjectStudents` | Collaborators record internal/external affiliation; student participation links students to projects. |
| Work plan | `WorkPlanActivities`, `ProjectMilestones` | Activities belong to projects and include dates, responsibilities, and expected deliverables. |
| Budget and funding | `BudgetItems`, `FundingSources`, `ProjectFunding` | Budget items and source-specific funding amounts belong to a project. |
| Committee review | `Committees`, `CommitteeMembers`, `ProjectReviewAssignments`, `Reviews`, `CommitteeDecisions` | Assignments identify the submission, reviewer, and committee role; decisions reference the reviewed version. |
| Academic authorization | `AcademicAuthorizations` | Each decision references the committee-endorsed submission and authorized decision-maker. |
| Progress and outputs | `ProgressReports`, `ProgressReportActivities`, `ResearchOutputs` | Reports link to work-plan activities; outputs link to a project and may also link to a progress report. |
| Forms and attachments | `DocumentTemplates`, `TemplateVersions`, `CallDocumentRequirements`, `Documents`, `ProjectDocuments`, `ProgressReportDocuments`, `ResearchOutputDocuments` | Templates have versions; explicit association tables link attachments to their parent record with foreign keys. |
| History and accountability | `ProjectStatusHistory`, `AuditLogs` | Preserve workflow transitions and relevant changes, decisions, and downloads. |

Table names are a design proposal. Final columns, relationships, and scripts belong in the database implementation.

### Integrity and persistence rules

- Use primary keys, foreign keys, unique constraints, and indexes on the actual query paths.
- Require one principal investigator per project. Keep collaborator and student participation distinct from responsibility for the project.
- Store money as fixed-precision decimals with an explicit currency. Validate nonnegative amounts and define how budget totals and funding commitments are reconciled. Do not sum different currencies without an agreed conversion rule.
- Validate date ranges and submission windows. Define how administrators handle exceptions or amended calls.
- Store workflow decisions against the exact submission reviewed. Use a transaction to save each decision and its corresponding state/history change atomically.
- Use optimistic concurrency, such as SQL Server `rowversion`, to detect conflicting edits and prevent duplicate or stale decisions.
- Store timestamps consistently in UTC and format them for the user at the presentation layer.
- Store document metadata and storage keys in SQL Server; store file contents in private persistent storage. Preserve historical attachment versions referenced by a submission.
- Avoid cascading deletion of approved projects, decisions, evidence, and audit history. Define archival and retention rules before enabling permanent deletion.
- Store password hashes through the selected authentication implementation; never store plaintext passwords or default production credentials.

## API Responsibilities

The API will expose authenticated operations for profiles, proposals, reviews, authorization, progress, outputs, reference data, templates, and reporting. Exact routes will be defined during implementation.

- Use request and response DTOs rather than exposing EF Core entities directly.
- Validate required fields, participant references, date ranges, budget values, document requirements, and allowed state transitions on the server.
- Apply resource-level permissions when reading or modifying projects, CV data, reviews, and files.
- Support pagination and appropriate filters for project, researcher, call, status, and reporting lists.
- Return consistent error responses and meaningful HTTP status codes, including concurrency conflicts.
- Record relevant changes and decisions in audit history. Notifications may report a saved decision, but email delivery must not determine whether the decision was committed.
- Provide OpenAPI documentation covering authentication, request/response contracts, and workflow errors.

## Documents, Evidence, and Exports

### Institutional forms

Administrators will publish versioned downloadable templates, including the rights assignment letter. Requirements can vary by call and workflow stage. When a researcher uploads a completed form, retain its template version and project association so reviewers can identify which requirement it satisfies.

### Private attachments

Support research protocols, completed forms, progress evidence, and research output files. Validate file sizes, allowed types, and filenames; assign server-generated storage keys and prevent path traversal. Keep private uploads outside the public web root and authorize every download through the backend.

Database and filesystem writes are not one transaction. Implement staged uploads or compensating cleanup so failed saves do not leave missing attachments or orphaned files. Back up metadata and file contents together.

### Reports and downloads

Provide permission-scoped Excel (`.xlsx`) and PDF exports for project summaries, participants, budgets/funding, review and authorization status, progress, and outputs. Authorized users must also be able to download the supporting evidence they are permitted to access.

Apply the same row-level and field-level permissions to exports as to normal API queries. Exclude restricted CV fields and review notes unless access is explicitly granted. Handle untrusted text safely in spreadsheet exports to prevent formula execution. Include report filters and generation timestamps, and audit report/evidence downloads.

## Local Development

### Prerequisites

- .NET SDK matching the project's target framework; the supplied technical baseline is .NET 8.
- SQL Server or SQL Server Express compatible with the project.
- Visual Studio Code with C# Dev Kit, or another .NET-compatible IDE.
- SQL Server Management Studio or an equivalent SQL client.
- The EF Core CLI version matching the project's EF Core packages, if using database scaffolding.

### Setup

1. Locate the API `.csproj` and confirm its framework and configuration keys.
2. Create the development database and apply the reviewed scripts under `database/` in their documented order. Those scripts are a development deliverable; their existence is not assumed here.
3. Configure a development connection string, authentication settings, private upload storage, and any email settings through local configuration or a secret store.
4. Open a terminal in the API project directory and run:

```bash
dotnet restore
dotnet build
dotnet run
```

Use the address printed by the application or configured in `Properties/launchSettings.json`. Access Swagger/OpenAPI only if it is enabled by the project's startup configuration.

For a local HTTPS certificate, when required:

```bash
dotnet dev-certs https --trust
```

Example development connection string for Windows authentication:

```text
Server=localhost\SQLEXPRESS;Database=TecNMResearchDB;Trusted_Connection=True;TrustServerCertificate=True;
```

This is a local-development example. Configure production certificate validation and authentication for the actual server.

Provision the initial administrator through a controlled setup procedure to be implemented. Do not assume a seeded administrator email or password.

## Database Change Management

The proposed approach is database-first: versioned SQL scripts define schema changes, and EF Core entities are refreshed through scaffolding. Confirm this approach before implementation and avoid maintaining a competing EF migration history for the same schema.

For each schema update:

1. Add a reviewed, ordered SQL script under `database/`, including required data transformations.
2. Test it on a disposable database and document recovery or rollback steps.
3. Apply it to the development database.
4. Refresh `AppDbContext` and entities with compatible EF Core tooling.
5. Review the generated diff, rebuild, and run affected tests.

Example scaffold command, run from the API project directory after configuring `ConnectionStrings:DefaultConnection`:

```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer --context AppDbContext --context-dir Data --output-dir Entities --no-onconfiguring
```

For regeneration, add `--force` only after committing or backing up the current files and reviewing the overwrite scope. Keep custom logic outside generated files, using partial classes or separate configuration where appropriate.

## Production Deployment

The proposed hosting target is IIS on Windows Server with SQL Server and private persistent document storage.

1. Build and test the backend, then publish from the API project directory:

```bash
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish
```

2. Install the Hosting Bundle compatible with the target framework and configure an IIS application pool with **No Managed Code**.
3. Deploy the published API to its application directory. Retain or generate the appropriate `web.config` for the actual assembly and hosting model.
4. Configure the production SQL connection, token signing settings, allowed frontend origins, upload path, and email provider using the application's implemented settings. Keep production secrets out of source control.
5. Grant the application identity only the required database and private-storage permissions. Keep uploads outside both the public web root and the deployment directory.
6. Back up the database and evidence storage before applying reviewed schema changes. Coordinate deployment with the required schema version.
7. Enable HTTPS, configure logs and health monitoring, and verify authentication, authorization, proposal submission, review, file access, and exports.
8. Test recovery of both SQL data and private files. A redeployment must not erase uploaded protocols, forms, or evidence.

Frontend build and IIS routing instructions are outside the scope of this backend README.

## Testing

Unit tests will cover business rules independently of HTTP controllers wherever practical. Integration tests will verify API behavior and SQL Server persistence, including constraints, transactions, and concurrency.

Priority scenarios include:

- A researcher cannot edit another researcher's CV or an unauthorized project.
- A proposal cannot be submitted without the required fields, responsible researcher, and stage-specific attachments.
- A user cannot review an unassigned proposal or authorize a project before committee endorsement.
- Corrections preserve the previously reviewed submission and its attachment versions.
- Concurrent decisions cannot create contradictory project states or duplicate authorizations.
- Progress and output reporting follow the approved workflow rules.
- Report and evidence downloads enforce record and field permissions.
- File validation rejects disallowed uploads, and cleanup handles failed saves.
- Database scripts work in their documented order on an empty database and on the supported prior schema.

## Development Checklist

All tasks below are pending verification or implementation.

### 1. Confirm requirements and workflow

- [ ] Agree on basic CV fields and required protocol sections.
- [ ] Define required objectives, work-plan fields, participant details, budget categories, and funding fields.
- [ ] Confirm how research lines, academic programs, and calls relate to projects, including whether a call is mandatory.
- [ ] Define committee membership, assignment scope, review order, endorsement rules, and conflicts of interest.
- [ ] Confirm Academic Subdirectorate decision rules and correction routing.
- [ ] Define progress reporting periods, output types, closure, cancellation, and retention rules.
- [ ] Approve the role/permission matrix and report visibility rules.
- [ ] Confirm required forms and evidence by call and workflow stage.

### 2. Establish backend and database foundations

- [ ] Verify the actual framework, dependencies, project name, and hosting baseline.
- [ ] Implement the feature folders and controller/service/DTO boundaries.
- [ ] Implement authentication, account activation, password recovery, and controlled administrator provisioning.
- [ ] Implement role policies, project-level permissions, validation, error handling, and audit infrastructure.
- [ ] Create versioned database scripts, foreign keys, indexes, constraints, and initial reference data.
- [ ] Configure EF Core mappings, concurrency handling, and database-first regeneration.
- [ ] Set up unit and integration test projects and continuous build checks.

### 3. Implement profiles and reference data

- [ ] Implement researcher profiles and basic CV management.
- [ ] Implement student records and internal/external collaborator information.
- [ ] Implement research line, academic program, and output type management.
- [ ] Implement calls for proposals, submission windows, and document requirements.
- [ ] Implement template publication, versioning, and authorized downloads.

### 4. Implement project registration

- [ ] Implement project drafts, protocol sections, objectives, and principal investigator assignment.
- [ ] Implement supporting students and internal/external collaborators.
- [ ] Implement work plans, activities, milestones, and deadlines.
- [ ] Implement budget items, funding sources, amounts, currencies, and consistency validation.
- [ ] Implement private attachments and completed institutional forms.
- [ ] Implement submission validation and immutable proposal versions.

### 5. Implement review and authorization

- [ ] Implement committee setup and reviewer assignments.
- [ ] Implement president, secretary, and program head reviews and observations.
- [ ] Implement correction requests, resubmission, and committee decisions.
- [ ] Implement subsequent Academic Subdirectorate authorization or rejection.
- [ ] Implement transactional state changes, decision history, and concurrency protection.
- [ ] Implement account and workflow notifications with delivery failure handling.

### 6. Implement progress and results

- [ ] Implement progress reports linked to work-plan activities.
- [ ] Implement research outputs and supporting evidence.
- [ ] Implement project closure and cancellation under the agreed institutional rules.
- [ ] Preserve evidence versions and a complete project history.

### 7. Implement reporting and downloads

- [ ] Implement filtered, paginated reporting queries.
- [ ] Implement Excel exports with safe handling of untrusted cell text.
- [ ] Implement PDF reports for the approved report layouts.
- [ ] Implement permission-scoped evidence downloads and download auditing.
- [ ] Verify that exports exclude unauthorized records and restricted fields.

### 8. Validate and deploy

- [ ] Complete unit tests for validation, permissions, calculations, and workflow transitions.
- [ ] Complete SQL Server/API integration tests for constraints, transactions, concurrency, uploads, and exports.
- [ ] Publish OpenAPI documentation and configuration/setup instructions matching the implemented code.
- [ ] Test database installation and upgrades with realistic sample projects.
- [ ] Configure IIS, HTTPS, production secrets, logging, and private file storage.
- [ ] Verify coordinated database/file backup and restoration.
- [ ] Complete acceptance testing with researchers, committee members, the Academic Subdirectorate, and administrators.
- [ ] Update this README to distinguish delivered functionality from remaining work.
