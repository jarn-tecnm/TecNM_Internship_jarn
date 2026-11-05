using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Entities;

namespace TecNM.Research.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicAuthorizationsHistory> AcademicAuthorizationsHistories { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<CAcademicAuthorizationStatus> CAcademicAuthorizationStatuses { get; set; }

    public virtual DbSet<CAcademicField> CAcademicFields { get; set; }

    public virtual DbSet<CAcademicProgram> CAcademicPrograms { get; set; }

    public virtual DbSet<CBudgetCategory> CBudgetCategories { get; set; }

    public virtual DbSet<CCall> CCalls { get; set; }

    public virtual DbSet<CCallType> CCallTypes { get; set; }

    public virtual DbSet<CCommitteeRole> CCommitteeRoles { get; set; }

    public virtual DbSet<CCurrency> CCurrencies { get; set; }

    public virtual DbSet<CDegreeLevel> CDegreeLevels { get; set; }

    public virtual DbSet<CDocumentTemplate> CDocumentTemplates { get; set; }

    public virtual DbSet<CDocumentType> CDocumentTypes { get; set; }

    public virtual DbSet<CFundingSource> CFundingSources { get; set; }

    public virtual DbSet<CMilestoneStatus> CMilestoneStatuses { get; set; }

    public virtual DbSet<CObjectiveStatus> CObjectiveStatuses { get; set; }

    public virtual DbSet<COrganization> COrganizations { get; set; }

    public virtual DbSet<COrganizationType> COrganizationTypes { get; set; }

    public virtual DbSet<CPermission> CPermissions { get; set; }

    public virtual DbSet<CProjectStatus> CProjectStatuses { get; set; }

    public virtual DbSet<CResearchLine> CResearchLines { get; set; }

    public virtual DbSet<CResearchOutputsType> CResearchOutputsTypes { get; set; }

    public virtual DbSet<CResearchersRole> CResearchersRoles { get; set; }

    public virtual DbSet<CReviewOutcome> CReviewOutcomes { get; set; }

    public virtual DbSet<CReviewStatus> CReviewStatuses { get; set; }

    public virtual DbSet<CRole> CRoles { get; set; }

    public virtual DbSet<CTaskStatus> CTaskStatuses { get; set; }

    public virtual DbSet<CallsDocumentRequirement> CallsDocumentRequirements { get; set; }

    public virtual DbSet<CallsRequirement> CallsRequirements { get; set; }

    public virtual DbSet<Committee> Committees { get; set; }

    public virtual DbSet<CommitteesMember> CommitteesMembers { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<ExternalResearchOutput> ExternalResearchOutputs { get; set; }

    public virtual DbSet<ExternalResearchOutputsDocument> ExternalResearchOutputsDocuments { get; set; }

    public virtual DbSet<ExternalResearchOutputsResearcher> ExternalResearchOutputsResearchers { get; set; }

    public virtual DbSet<Milestone> Milestones { get; set; }

    public virtual DbSet<MilestonesCurrentStatus> MilestonesCurrentStatuses { get; set; }

    public virtual DbSet<MilestonesStatusHistory> MilestonesStatusHistories { get; set; }

    public virtual DbSet<MilestonesStatusHistoryDocument> MilestonesStatusHistoryDocuments { get; set; }

    public virtual DbSet<Objective> Objectives { get; set; }

    public virtual DbSet<ObjectivesCurrentStatus> ObjectivesCurrentStatuses { get; set; }

    public virtual DbSet<ObjectivesStatusHistory> ObjectivesStatusHistories { get; set; }

    public virtual DbSet<ObjectivesStatusHistoryDocument> ObjectivesStatusHistoryDocuments { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectsApplication> ProjectsApplications { get; set; }

    public virtual DbSet<ProjectsApplicationsDocument> ProjectsApplicationsDocuments { get; set; }

    public virtual DbSet<ProjectsBudgetDocument> ProjectsBudgetDocuments { get; set; }

    public virtual DbSet<ProjectsBudgetItem> ProjectsBudgetItems { get; set; }

    public virtual DbSet<ProjectsCurrentStatus> ProjectsCurrentStatuses { get; set; }

    public virtual DbSet<ProjectsDocument> ProjectsDocuments { get; set; }

    public virtual DbSet<ProjectsExpense> ProjectsExpenses { get; set; }

    public virtual DbSet<ProjectsExpensesDocument> ProjectsExpensesDocuments { get; set; }

    public virtual DbSet<ProjectsFunding> ProjectsFundings { get; set; }

    public virtual DbSet<ProjectsFundingDocument> ProjectsFundingDocuments { get; set; }

    public virtual DbSet<ProjectsLead> ProjectsLeads { get; set; }

    public virtual DbSet<ProjectsResearchLine> ProjectsResearchLines { get; set; }

    public virtual DbSet<ProjectsResearcher> ProjectsResearchers { get; set; }

    public virtual DbSet<ProjectsReview> ProjectsReviews { get; set; }

    public virtual DbSet<ProjectsStatusHistory> ProjectsStatusHistories { get; set; }

    public virtual DbSet<ProjectsStudent> ProjectsStudents { get; set; }

    public virtual DbSet<ProjectsSubmission> ProjectsSubmissions { get; set; }

    public virtual DbSet<ResearchOutput> ResearchOutputs { get; set; }

    public virtual DbSet<ResearchOutputsDocument> ResearchOutputsDocuments { get; set; }

    public virtual DbSet<ResearchOutputsResearcher> ResearchOutputsResearchers { get; set; }

    public virtual DbSet<Researcher> Researchers { get; set; }

    public virtual DbSet<ResearchersDegree> ResearchersDegrees { get; set; }

    public virtual DbSet<ResearchersExperience> ResearchersExperiences { get; set; }

    public virtual DbSet<ResearchersOrganization> ResearchersOrganizations { get; set; }

    public virtual DbSet<ResearchersResearchLine> ResearchersResearchLines { get; set; }

    public virtual DbSet<RolesPermission> RolesPermissions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<SubmissionsDocument> SubmissionsDocuments { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TasksCurrentStatus> TasksCurrentStatuses { get; set; }

    public virtual DbSet<TasksStatusHistory> TasksStatusHistories { get; set; }

    public virtual DbSet<TasksStatusHistoryDocument> TasksStatusHistoryDocuments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersRole> UsersRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicAuthorizationsHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_authorizations_history_pkey");

            entity.ToTable("academic_authorizations_history", "reviews");

            entity.HasIndex(e => e.SubmissionId, "fk_157195_idx");

            entity.HasIndex(e => e.StatusId, "fk_157200_idx");

            entity.HasIndex(e => e.AuthorizedBy, "fk_157205_idx");

            entity.HasIndex(e => e.DocumentId, "fk_157210_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorizedBy).HasColumnName("authorized_by");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DecidedAt).HasColumnName("decided_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Rationale).HasColumnName("rationale");
            entity.Property(e => e.ReplacedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("replaced_at");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.AuthorizedByNavigation).WithMany(p => p.AcademicAuthorizationsHistories)
                .HasForeignKey(d => d.AuthorizedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_authorized_by_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.AcademicAuthorizationsHistories)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_document_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.AcademicAuthorizationsHistories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_status_id_fkey");

            entity.HasOne(d => d.Submission).WithMany(p => p.AcademicAuthorizationsHistories)
                .HasForeignKey(d => d.SubmissionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_submission_id_fkey");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_logs_pkey");

            entity.ToTable("audit_logs", "access");

            entity.HasIndex(e => e.ActorUserId, "fk_157241_idx");

            entity.HasIndex(e => e.ProjectId, "fk_157246_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasColumnName("action");
            entity.Property(e => e.ActorUserId).HasColumnName("actor_user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Details)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb")
                .HasColumnName("details");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntitySchema)
                .HasMaxLength(63)
                .HasColumnName("entity_schema");
            entity.Property(e => e.EntityTable)
                .HasMaxLength(63)
                .HasColumnName("entity_table");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OccurredAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("occurred_at");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.RequestId)
                .HasMaxLength(100)
                .HasColumnName("request_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ActorUser).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.ActorUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("audit_logs_actor_user_id_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("audit_logs_project_id_fkey");
        });

        modelBuilder.Entity<CAcademicAuthorizationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_academic_authorization_statuses_pkey");

            entity.ToTable("c_academic_authorization_statuses", "reviews");

            entity.HasIndex(e => e.Code, "c_academic_authorization_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CAcademicField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_academic_fields_pkey");

            entity.ToTable("c_academic_fields", "academic");

            entity.HasIndex(e => e.Code, "c_academic_fields_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CAcademicProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_academic_programs_pkey");

            entity.ToTable("c_academic_programs", "academic");

            entity.HasIndex(e => e.Code, "c_academic_programs_code_key").IsUnique();

            entity.HasIndex(e => e.OrganizationId, "fk_155003_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Organization).WithMany(p => p.CAcademicPrograms)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_academic_programs_organization_id_fkey");
        });

        modelBuilder.Entity<CBudgetCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_budget_categories_pkey");

            entity.ToTable("c_budget_categories", "research");

            entity.HasIndex(e => e.Code, "c_budget_categories_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CCall>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_calls_pkey");

            entity.ToTable("c_calls", "calls");

            entity.HasIndex(e => e.Code, "c_calls_code_key").IsUnique();

            entity.HasIndex(e => e.OrganizationId, "fk_155335_idx");

            entity.HasIndex(e => e.CallTypeId, "fk_155340_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CallTypeId).HasColumnName("call_type_id");
            entity.Property(e => e.ClosesAt).HasColumnName("closes_at");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OpensAt).HasColumnName("opens_at");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CallType).WithMany(p => p.CCalls)
                .HasForeignKey(d => d.CallTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_calls_call_type_id_fkey");

            entity.HasOne(d => d.Organization).WithMany(p => p.CCalls)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_calls_organization_id_fkey");
        });

        modelBuilder.Entity<CCallType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_call_types_pkey");

            entity.ToTable("c_call_types", "calls");

            entity.HasIndex(e => e.Code, "c_call_types_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CCommitteeRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_committee_roles_pkey");

            entity.ToTable("c_committee_roles", "reviews");

            entity.HasIndex(e => e.Code, "c_committee_roles_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CCurrency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_currencies_pkey");

            entity.ToTable("c_currencies", "research");

            entity.HasIndex(e => e.Code, "c_currencies_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CDegreeLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_degree_levels_pkey");

            entity.ToTable("c_degree_levels", "academic");

            entity.HasIndex(e => e.Code, "c_degree_levels_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CDocumentTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_document_templates_pkey");

            entity.ToTable("c_document_templates", "files");

            entity.HasIndex(e => new { e.Code, e.VersionNumber }, "c_document_templates_code_version_number_key").IsUnique();

            entity.HasIndex(e => e.Code, "c_document_templates_current_uq")
                .IsUnique()
                .HasFilter("(is_current AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.StorageKey, "c_document_templates_storage_key_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.MediaType)
                .HasMaxLength(150)
                .HasColumnName("media_type");
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OriginalFileName)
                .HasMaxLength(300)
                .HasColumnName("original_file_name");
            entity.Property(e => e.Sha256)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("sha256");
            entity.Property(e => e.SizeBytes).HasColumnName("size_bytes");
            entity.Property(e => e.StorageKey).HasColumnName("storage_key");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber).HasColumnName("version_number");
        });

        modelBuilder.Entity<CDocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_document_types_pkey");

            entity.ToTable("c_document_types", "files");

            entity.HasIndex(e => e.Code, "c_document_types_code_key").IsUnique();

            entity.HasIndex(e => e.TemplateId, "fk_155404_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.TemplateId).HasColumnName("template_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Template).WithMany(p => p.CDocumentTypes)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_document_types_template_id_fkey");
        });

        modelBuilder.Entity<CFundingSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_funding_sources_pkey");

            entity.ToTable("c_funding_sources", "research");

            entity.HasIndex(e => e.Code, "c_funding_sources_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CMilestoneStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_milestone_statuses_pkey");

            entity.ToTable("c_milestone_statuses", "research");

            entity.HasIndex(e => e.Code, "c_milestone_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CObjectiveStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_objective_statuses_pkey");

            entity.ToTable("c_objective_statuses", "research");

            entity.HasIndex(e => e.Code, "c_objective_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<COrganization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_organizations_pkey");

            entity.ToTable("c_organizations", "academic");

            entity.HasIndex(e => e.Code, "c_organizations_code_key").IsUnique();

            entity.HasIndex(e => e.OrganizationTypeId, "fk_154972_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("country_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrganizationTypeId).HasColumnName("organization_type_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.OrganizationType).WithMany(p => p.COrganizations)
                .HasForeignKey(d => d.OrganizationTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_organizations_organization_type_id_fkey");
        });

        modelBuilder.Entity<COrganizationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_organization_types_pkey");

            entity.ToTable("c_organization_types", "academic");

            entity.HasIndex(e => e.Code, "c_organization_types_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_permissions_pkey");

            entity.ToTable("c_permissions", "access");

            entity.HasIndex(e => e.Code, "c_permissions_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CProjectStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_project_statuses_pkey");

            entity.ToTable("c_project_statuses", "research");

            entity.HasIndex(e => e.Code, "c_project_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CResearchLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_research_lines_pkey");

            entity.ToTable("c_research_lines", "academic");

            entity.HasIndex(e => e.Code, "c_research_lines_code_key").IsUnique();

            entity.HasIndex(e => e.AcademicFieldId, "fk_155059_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicFieldId).HasColumnName("academic_field_id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.AcademicField).WithMany(p => p.CResearchLines)
                .HasForeignKey(d => d.AcademicFieldId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_research_lines_academic_field_id_fkey");
        });

        modelBuilder.Entity<CResearchOutputsType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_research_outputs_types_pkey");

            entity.ToTable("c_research_outputs_types", "research");

            entity.HasIndex(e => e.Code, "c_research_outputs_types_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CResearchersRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_researchers_roles_pkey");

            entity.ToTable("c_researchers_roles", "research");

            entity.HasIndex(e => e.Code, "c_researchers_roles_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CReviewOutcome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_review_outcomes_pkey");

            entity.ToTable("c_review_outcomes", "reviews");

            entity.HasIndex(e => e.Code, "c_review_outcomes_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CReviewStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_review_statuses_pkey");

            entity.ToTable("c_review_statuses", "reviews");

            entity.HasIndex(e => e.Code, "c_review_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_roles_pkey");

            entity.ToTable("c_roles", "access");

            entity.HasIndex(e => e.Code, "c_roles_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CTaskStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("c_task_statuses_pkey");

            entity.ToTable("c_task_statuses", "research");

            entity.HasIndex(e => e.Code, "c_task_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(80)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<CallsDocumentRequirement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("calls_document_requirements_pkey");

            entity.ToTable("calls_document_requirements", "calls");

            entity.HasIndex(e => new { e.CallId, e.DocumentTypeId, e.Title, e.WorkflowStage }, "calls_document_requirements_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.CallId, "fk_155479_idx");

            entity.HasIndex(e => e.DocumentTypeId, "fk_155484_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CallId).HasColumnName("call_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(true)
                .HasColumnName("is_required");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.WorkflowStage)
                .HasMaxLength(40)
                .HasColumnName("workflow_stage");

            entity.HasOne(d => d.Call).WithMany(p => p.CallsDocumentRequirements)
                .HasForeignKey(d => d.CallId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("calls_document_requirements_call_id_fkey");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.CallsDocumentRequirements)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("calls_document_requirements_document_type_id_fkey");
        });

        modelBuilder.Entity<CallsRequirement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("calls_requirements_pkey");

            entity.ToTable("calls_requirements", "calls");

            entity.HasIndex(e => e.CallId, "fk_155513_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CallId).HasColumnName("call_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(true)
                .HasColumnName("is_required");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Call).WithMany(p => p.CallsRequirements)
                .HasForeignKey(d => d.CallId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("calls_requirements_call_id_fkey");
        });

        modelBuilder.Entity<Committee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("committees_pkey");

            entity.ToTable("committees", "reviews");

            entity.HasIndex(e => e.AcademicProgramId, "fk_156973_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicProgramId).HasColumnName("academic_program_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.AcademicProgram).WithMany(p => p.Committees)
                .HasForeignKey(d => d.AcademicProgramId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_academic_program_id_fkey");
        });

        modelBuilder.Entity<CommitteesMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("committees_members_pkey");

            entity.ToTable("committees_members", "reviews");

            entity.HasIndex(e => new { e.Id, e.CommitteeId }, "committees_members_id_committee_id_key").IsUnique();

            entity.HasIndex(e => new { e.CommitteeId, e.UserId }, "committees_members_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.CommitteeId, e.CommitteeRoleId }, "committees_members_role_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.CommitteeId, "fk_157004_idx");

            entity.HasIndex(e => e.UserId, "fk_157009_idx");

            entity.HasIndex(e => e.CommitteeRoleId, "fk_157014_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommitteeId).HasColumnName("committee_id");
            entity.Property(e => e.CommitteeRoleId).HasColumnName("committee_role_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Committee).WithMany(p => p.CommitteesMembers)
                .HasForeignKey(d => d.CommitteeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_members_committee_id_fkey");

            entity.HasOne(d => d.CommitteeRole).WithMany(p => p.CommitteesMembers)
                .HasForeignKey(d => d.CommitteeRoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_members_committee_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.CommitteesMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_members_user_id_fkey");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("documents_pkey");

            entity.ToTable("documents", "files");

            entity.HasIndex(e => new { e.Id, e.DocumentTypeId }, "documents_id_document_type_id_key").IsUnique();

            entity.HasIndex(e => e.StorageKey, "documents_storage_key_key").IsUnique();

            entity.HasIndex(e => e.DocumentTypeId, "fk_155443_idx");

            entity.HasIndex(e => e.UploadedBy, "fk_155448_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.MediaType)
                .HasMaxLength(150)
                .HasColumnName("media_type");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OriginalFileName)
                .HasMaxLength(300)
                .HasColumnName("original_file_name");
            entity.Property(e => e.Sha256)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("sha256");
            entity.Property(e => e.SizeBytes).HasColumnName("size_bytes");
            entity.Property(e => e.StorageKey).HasColumnName("storage_key");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("uploaded_at");
            entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("documents_document_type_id_fkey");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("documents_uploaded_by_fkey");
        });

        modelBuilder.Entity<ExternalResearchOutput>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("external_research_outputs_pkey");

            entity.ToTable("external_research_outputs", "academic");

            entity.HasIndex(e => e.OutputTypeId, "fk_156774_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BibliographicCitation).HasColumnName("bibliographic_citation");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Doi)
                .HasMaxLength(250)
                .HasColumnName("doi");
            entity.Property(e => e.ExternalUrl).HasColumnName("external_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OutputTypeId).HasColumnName("output_type_id");
            entity.Property(e => e.ProducedOn).HasColumnName("produced_on");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.OutputType).WithMany(p => p.ExternalResearchOutputs)
                .HasForeignKey(d => d.OutputTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("external_research_outputs_output_type_id_fkey");
        });

        modelBuilder.Entity<ExternalResearchOutputsDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("external_research_outputs_documents_pkey");

            entity.ToTable("external_research_outputs_documents", "academic");

            entity.HasIndex(e => e.ExternalResearchOutputId, "fk_156843_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156848_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.ExternalResearchOutputId).HasColumnName("external_research_output_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.ExternalResearchOutput).WithMany(p => p.ExternalResearchOutputsDocuments)
                .HasForeignKey(d => d.ExternalResearchOutputId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("external_research_outputs_docu_external_research_output_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ExternalResearchOutputsDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("external_research_outputs_doc_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<ExternalResearchOutputsResearcher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("external_research_outputs_researchers_pkey");

            entity.ToTable("external_research_outputs_researchers", "academic");

            entity.HasIndex(e => new { e.ExternalResearchOutputId, e.AuthorOrder }, "external_research_outputs_researchers_author_order_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.ExternalResearchOutputId, e.ResearcherId }, "external_research_outputs_researchers_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.ExternalResearchOutputId, "fk_156804_idx");

            entity.HasIndex(e => e.ResearcherId, "fk_156809_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorOrder).HasColumnName("author_order");
            entity.Property(e => e.Contribution).HasColumnName("contribution");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.ExternalResearchOutputId).HasColumnName("external_research_output_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCorrespondingAuthor).HasColumnName("is_corresponding_author");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ExternalResearchOutput).WithMany(p => p.ExternalResearchOutputsResearchers)
                .HasForeignKey(d => d.ExternalResearchOutputId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("external_research_outputs_rese_external_research_output_id_fkey");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ExternalResearchOutputsResearchers)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("external_research_outputs_researchers_researcher_id_fkey");
        });

        modelBuilder.Entity<Milestone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("milestones_pkey");

            entity.ToTable("milestones", "research");

            entity.HasIndex(e => e.ObjectiveId, "fk_155970_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DueOn).HasColumnName("due_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ObjectiveId).HasColumnName("objective_id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Objective).WithMany(p => p.Milestones)
                .HasForeignKey(d => d.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_objective_id_fkey");
        });

        modelBuilder.Entity<MilestonesCurrentStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("milestones_current_status", "research");

            entity.Property(e => e.ChangedAt).HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CompletionPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("completion_percentage");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsVisible).HasColumnName("is_visible");
            entity.Property(e => e.MilestoneId).HasColumnName("milestone_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<MilestonesStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("milestones_status_history_pkey");

            entity.ToTable("milestones_status_history", "research");

            entity.HasIndex(e => e.MilestoneId, "fk_156159_idx");

            entity.HasIndex(e => e.StatusId, "fk_156164_idx");

            entity.HasIndex(e => e.ChangedBy, "fk_156169_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CompletionPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("completion_percentage");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.MilestoneId).HasColumnName("milestone_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.MilestonesStatusHistories)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_changed_by_fkey");

            entity.HasOne(d => d.Milestone).WithMany(p => p.MilestonesStatusHistories)
                .HasForeignKey(d => d.MilestoneId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_milestone_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.MilestonesStatusHistories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_status_id_fkey");
        });

        modelBuilder.Entity<MilestonesStatusHistoryDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("milestones_status_history_documents_pkey");

            entity.ToTable("milestones_status_history_documents", "research");

            entity.HasIndex(e => e.StatusHistoryId, "fk_156203_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156208_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StatusHistoryId).HasColumnName("status_history_id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.StatusHistory).WithMany(p => p.MilestonesStatusHistoryDocuments)
                .HasForeignKey(d => d.StatusHistoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_documents_status_history_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.MilestonesStatusHistoryDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("milestones_status_history_doc_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<Objective>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("objectives_pkey");

            entity.ToTable("objectives", "research");

            entity.HasIndex(e => e.ProjectId, "fk_155943_idx");

            entity.HasIndex(e => e.ProjectId, "objectives_general_uq")
                .IsUnique()
                .HasFilter("(((objective_type)::text = 'general'::text) AND is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.Id, e.ProjectId }, "objectives_id_project_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ObjectiveType)
                .HasMaxLength(10)
                .HasColumnName("objective_type");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Project).WithOne(p => p.Objective)
                .HasForeignKey<Objective>(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_project_id_fkey");
        });

        modelBuilder.Entity<ObjectivesCurrentStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("objectives_current_status", "research");

            entity.Property(e => e.ChangedAt).HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CompletionPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("completion_percentage");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsVisible).HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ObjectiveId).HasColumnName("objective_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<ObjectivesStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("objectives_status_history_pkey");

            entity.ToTable("objectives_status_history", "research");

            entity.HasIndex(e => e.ObjectiveId, "fk_156077_idx");

            entity.HasIndex(e => e.StatusId, "fk_156082_idx");

            entity.HasIndex(e => e.ChangedBy, "fk_156087_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CompletionPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("completion_percentage");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ObjectiveId).HasColumnName("objective_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.ObjectivesStatusHistories)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_changed_by_fkey");

            entity.HasOne(d => d.Objective).WithMany(p => p.ObjectivesStatusHistories)
                .HasForeignKey(d => d.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_objective_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ObjectivesStatusHistories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_status_id_fkey");
        });

        modelBuilder.Entity<ObjectivesStatusHistoryDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("objectives_status_history_documents_pkey");

            entity.ToTable("objectives_status_history_documents", "research");

            entity.HasIndex(e => e.StatusHistoryId, "fk_156121_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156126_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StatusHistoryId).HasColumnName("status_history_id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.StatusHistory).WithMany(p => p.ObjectivesStatusHistoryDocuments)
                .HasForeignKey(d => d.StatusHistoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_documents_status_history_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ObjectivesStatusHistoryDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("objectives_status_history_doc_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_pkey");

            entity.ToTable("projects", "research");

            entity.HasIndex(e => e.CurrencyId, "fk_155777_idx");

            entity.HasIndex(e => e.ReferenceCode, "projects_reference_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abstract).HasColumnName("abstract");
            entity.Property(e => e.BibliographicReferences).HasColumnName("bibliographic_references");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.ExpectedResults).HasColumnName("expected_results");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Justification).HasColumnName("justification");
            entity.Property(e => e.Methodology).HasColumnName("methodology");
            entity.Property(e => e.MilestonesProgressPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("milestones_progress_percentage");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ObjectivesProgressPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("objectives_progress_percentage");
            entity.Property(e => e.ProblemStatement).HasColumnName("problem_statement");
            entity.Property(e => e.ReferenceCode)
                .HasMaxLength(100)
                .HasColumnName("reference_code");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.TasksProgressPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("tasks_progress_percentage");
            entity.Property(e => e.TheoreticalFramework).HasColumnName("theoretical_framework");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Currency).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_currency_id_fkey");
        });

        modelBuilder.Entity<ProjectsApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_applications_pkey");

            entity.ToTable("projects_applications", "research");

            entity.HasIndex(e => e.ProjectId, "fk_156362_idx");

            entity.HasIndex(e => e.CallId, "fk_156367_idx");

            entity.HasIndex(e => new { e.Id, e.ProjectId }, "projects_applications_id_project_id_key").IsUnique();

            entity.HasIndex(e => new { e.ProjectId, e.CallId }, "projects_applications_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicationNumber)
                .HasMaxLength(100)
                .HasColumnName("application_number");
            entity.Property(e => e.AppliedAt).HasColumnName("applied_at");
            entity.Property(e => e.CallId).HasColumnName("call_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.RequestedAmount)
                .HasPrecision(18, 2)
                .HasColumnName("requested_amount");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Call).WithMany(p => p.ProjectsApplications)
                .HasForeignKey(d => d.CallId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_applications_call_id_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsApplications)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_applications_project_id_fkey");
        });

        modelBuilder.Entity<ProjectsApplicationsDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_applications_documents_pkey");

            entity.ToTable("projects_applications_documents", "research");

            entity.HasIndex(e => e.ApplicationId, "fk_156401_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156406_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.Application).WithMany(p => p.ProjectsApplicationsDocuments)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_applications_documents_application_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ProjectsApplicationsDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_applications_documen_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<ProjectsBudgetDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_budget_documents_pkey");

            entity.ToTable("projects_budget_documents", "research");

            entity.HasIndex(e => e.BudgetItemId, "fk_156479_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156484_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BudgetItemId).HasColumnName("budget_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.BudgetItem).WithMany(p => p.ProjectsBudgetDocuments)
                .HasForeignKey(d => d.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_budget_documents_budget_item_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ProjectsBudgetDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_budget_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<ProjectsBudgetItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_budget_items_pkey");

            entity.ToTable("projects_budget_items", "research");

            entity.HasIndex(e => e.ProjectId, "fk_156440_idx");

            entity.HasIndex(e => e.BudgetCategoryId, "fk_156445_idx");

            entity.HasIndex(e => new { e.Id, e.ProjectId }, "projects_budget_items_id_project_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BudgetCategoryId).HasColumnName("budget_category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(24, 2)
                .HasComputedColumnSql("round((quantity * unit_cost), 2)", true)
                .HasColumnName("total_amount");
            entity.Property(e => e.UnitCost)
                .HasPrecision(18, 2)
                .HasColumnName("unit_cost");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.BudgetCategory).WithMany(p => p.ProjectsBudgetItems)
                .HasForeignKey(d => d.BudgetCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_budget_items_budget_category_id_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsBudgetItems)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_budget_items_project_id_fkey");
        });

        modelBuilder.Entity<ProjectsCurrentStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("projects_current_status", "research");

            entity.Property(e => e.ChangedAt).HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsVisible).HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<ProjectsDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_documents_pkey");

            entity.ToTable("projects_documents", "research");

            entity.HasIndex(e => e.ProjectId, "fk_156326_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156331_idx");

            entity.HasIndex(e => new { e.Id, e.ProjectId, e.DocumentId }, "projects_documents_id_project_id_document_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsDocuments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_documents_project_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ProjectsDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<ProjectsExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_expenses_pkey");

            entity.ToTable("projects_expenses", "research");

            entity.HasIndex(e => e.BudgetItemId, "fk_156598_idx");

            entity.HasIndex(e => e.FundingId, "fk_156603_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.BudgetItemId).HasColumnName("budget_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.FundingId).HasColumnName("funding_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ReceiptNumber)
                .HasMaxLength(100)
                .HasColumnName("receipt_number");
            entity.Property(e => e.SpentOn).HasColumnName("spent_on");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(300)
                .HasColumnName("supplier_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.BudgetItem).WithMany(p => p.ProjectsExpenses)
                .HasForeignKey(d => d.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_expenses_budget_item_id_fkey");

            entity.HasOne(d => d.Funding).WithMany(p => p.ProjectsExpenses)
                .HasForeignKey(d => d.FundingId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_expenses_funding_id_fkey");
        });

        modelBuilder.Entity<ProjectsExpensesDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_expenses_documents_pkey");

            entity.ToTable("projects_expenses_documents", "research");

            entity.HasIndex(e => e.ExpenseId, "fk_156637_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156642_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.ExpenseId).HasColumnName("expense_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.Expense).WithMany(p => p.ProjectsExpensesDocuments)
                .HasForeignKey(d => d.ExpenseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_expenses_documents_expense_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ProjectsExpensesDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_expenses_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<ProjectsFunding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_funding_pkey");

            entity.ToTable("projects_funding", "research");

            entity.HasIndex(e => e.ProjectId, "fk_156520_idx");

            entity.HasIndex(e => e.FundingSourceId, "fk_156525_idx");

            entity.HasIndex(e => new { e.ApplicationId, e.ProjectId }, "fk_156530_idx");

            entity.HasIndex(e => new { e.Id, e.ProjectId }, "projects_funding_id_project_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.ApprovedAmount)
                .HasPrecision(18, 2)
                .HasColumnName("approved_amount");
            entity.Property(e => e.AwardedOn).HasColumnName("awarded_on");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.FundingSourceId).HasColumnName("funding_source_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.ReceivedAmount)
                .HasPrecision(18, 2)
                .HasColumnName("received_amount");
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(100)
                .HasColumnName("reference_number");
            entity.Property(e => e.RequestedAmount)
                .HasPrecision(18, 2)
                .HasColumnName("requested_amount");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.FundingSource).WithMany(p => p.ProjectsFundings)
                .HasForeignKey(d => d.FundingSourceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_funding_funding_source_id_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsFundings)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_funding_project_id_fkey");

            entity.HasOne(d => d.ProjectsApplication).WithMany(p => p.ProjectsFundings)
                .HasPrincipalKey(p => new { p.Id, p.ProjectId })
                .HasForeignKey(d => new { d.ApplicationId, d.ProjectId })
                .HasConstraintName("projects_funding_application_id_project_id_fkey");
        });

        modelBuilder.Entity<ProjectsFundingDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_funding_documents_pkey");

            entity.ToTable("projects_funding_documents", "research");

            entity.HasIndex(e => e.FundingId, "fk_156564_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156569_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.FundingId).HasColumnName("funding_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.Funding).WithMany(p => p.ProjectsFundingDocuments)
                .HasForeignKey(d => d.FundingId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_funding_documents_funding_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ProjectsFundingDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_funding_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<ProjectsLead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_leads_pkey");

            entity.ToTable("projects_leads", "research");

            entity.HasIndex(e => e.ProjectResearcherId, "fk_155849_idx");

            entity.HasIndex(e => e.ProjectResearcherId, "projects_leads_participant_current_uq")
                .IsUnique()
                .HasFilter("(is_current AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangeReason).HasColumnName("change_reason");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsAt).HasColumnName("ends_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectResearcherId).HasColumnName("project_researcher_id");
            entity.Property(e => e.StartsAt).HasColumnName("starts_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ProjectResearcher).WithOne(p => p.ProjectsLead)
                .HasForeignKey<ProjectsLead>(d => d.ProjectResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_leads_project_researcher_id_fkey");
        });

        modelBuilder.Entity<ProjectsResearchLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_research_lines_pkey");

            entity.ToTable("projects_research_lines", "research");

            entity.HasIndex(e => e.ProjectId, "fk_155908_idx");

            entity.HasIndex(e => e.ResearchLineId, "fk_155913_idx");

            entity.HasIndex(e => new { e.ProjectId, e.ResearchLineId }, "projects_research_lines_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.ResearchLineId).HasColumnName("research_line_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsResearchLines)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_research_lines_project_id_fkey");

            entity.HasOne(d => d.ResearchLine).WithMany(p => p.ProjectsResearchLines)
                .HasForeignKey(d => d.ResearchLineId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_research_lines_research_line_id_fkey");
        });

        modelBuilder.Entity<ProjectsResearcher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_researchers_pkey");

            entity.ToTable("projects_researchers", "research");

            entity.HasIndex(e => e.ProjectId, "fk_155808_idx");

            entity.HasIndex(e => e.ResearcherId, "fk_155813_idx");

            entity.HasIndex(e => e.ResearcherRoleId, "fk_155818_idx");

            entity.HasIndex(e => new { e.Id, e.ProjectId }, "projects_researchers_id_project_id_key").IsUnique();

            entity.HasIndex(e => new { e.ProjectId, e.ResearcherId }, "projects_researchers_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contribution).HasColumnName("contribution");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");
            entity.Property(e => e.ResearcherRoleId).HasColumnName("researcher_role_id");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsResearchers)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_researchers_project_id_fkey");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ProjectsResearchers)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_researchers_researcher_id_fkey");

            entity.HasOne(d => d.ResearcherRole).WithMany(p => p.ProjectsResearchers)
                .HasForeignKey(d => d.ResearcherRoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_researchers_researcher_role_id_fkey");
        });

        modelBuilder.Entity<ProjectsReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_reviews_pkey");

            entity.ToTable("projects_reviews", "reviews");

            entity.HasIndex(e => e.StatusId, "fk_157148_idx");

            entity.HasIndex(e => e.OutcomeId, "fk_157153_idx");

            entity.HasIndex(e => new { e.SubmissionId, e.CommitteeId }, "fk_157158_idx");

            entity.HasIndex(e => new { e.CommitteeMemberId, e.CommitteeId }, "fk_157163_idx");

            entity.HasIndex(e => new { e.SubmissionId, e.CommitteeMemberId }, "projects_reviews_submission_id_committee_member_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Assessment).HasColumnName("assessment");
            entity.Property(e => e.CommitteeId).HasColumnName("committee_id");
            entity.Property(e => e.CommitteeMemberId).HasColumnName("committee_member_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OutcomeId).HasColumnName("outcome_id");
            entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Outcome).WithMany(p => p.ProjectsReviews)
                .HasForeignKey(d => d.OutcomeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_reviews_outcome_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ProjectsReviews)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_reviews_status_id_fkey");

            entity.HasOne(d => d.CommitteesMember).WithMany(p => p.ProjectsReviews)
                .HasPrincipalKey(p => new { p.Id, p.CommitteeId })
                .HasForeignKey(d => new { d.CommitteeMemberId, d.CommitteeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_reviews_committee_member_id_committee_id_fkey");

            entity.HasOne(d => d.ProjectsSubmission).WithMany(p => p.ProjectsReviews)
                .HasPrincipalKey(p => new { p.Id, p.CommitteeId })
                .HasForeignKey(d => new { d.SubmissionId, d.CommitteeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_reviews_submission_id_committee_id_fkey");
        });

        modelBuilder.Entity<ProjectsStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_status_history_pkey");

            entity.ToTable("projects_status_history", "research");

            entity.HasIndex(e => e.ProjectId, "fk_156034_idx");

            entity.HasIndex(e => e.StatusId, "fk_156039_idx");

            entity.HasIndex(e => e.ChangedBy, "fk_156044_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.ProjectsStatusHistories)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_status_history_changed_by_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsStatusHistories)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_status_history_project_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ProjectsStatusHistories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_status_history_status_id_fkey");
        });

        modelBuilder.Entity<ProjectsStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_students_pkey");

            entity.ToTable("projects_students", "research");

            entity.HasIndex(e => e.ProjectId, "fk_155877_idx");

            entity.HasIndex(e => e.StudentId, "fk_155882_idx");

            entity.HasIndex(e => new { e.ProjectId, e.StudentId }, "projects_students_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activities).HasColumnName("activities");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsStudents)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_students_project_id_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.ProjectsStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_students_student_id_fkey");
        });

        modelBuilder.Entity<ProjectsSubmission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_submissions_pkey");

            entity.ToTable("projects_submissions", "reviews");

            entity.HasIndex(e => e.ProjectId, "fk_157055_idx");

            entity.HasIndex(e => e.CommitteeId, "fk_157060_idx");

            entity.HasIndex(e => e.SubmittedBy, "fk_157065_idx");

            entity.HasIndex(e => e.AcademicStatusId, "fk_157070_idx");

            entity.HasIndex(e => e.AcademicAuthorizedBy, "fk_157075_idx");

            entity.HasIndex(e => e.AcademicDocumentId, "fk_157080_idx");

            entity.HasIndex(e => new { e.ApplicationId, e.ProjectId }, "fk_157085_idx");

            entity.HasIndex(e => new { e.Id, e.CommitteeId }, "projects_submissions_id_committee_id_key").IsUnique();

            entity.HasIndex(e => e.ProjectId, "projects_submissions_in_process_uq")
                .IsUnique()
                .HasFilter("(((processing_status)::text = 'in_process'::text) AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.ProjectId, e.VersionNumber }, "projects_submissions_project_id_version_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicAuthorizedBy).HasColumnName("academic_authorized_by");
            entity.Property(e => e.AcademicDecidedAt).HasColumnName("academic_decided_at");
            entity.Property(e => e.AcademicDocumentId).HasColumnName("academic_document_id");
            entity.Property(e => e.AcademicRationale).HasColumnName("academic_rationale");
            entity.Property(e => e.AcademicStatusId).HasColumnName("academic_status_id");
            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.CommitteeId).HasColumnName("committee_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProcessingStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'in_process'::character varying")
                .HasColumnName("processing_status");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Snapshot)
                .HasColumnType("jsonb")
                .HasColumnName("snapshot");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("submitted_at");
            entity.Property(e => e.SubmittedBy).HasColumnName("submitted_by");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber).HasColumnName("version_number");

            entity.HasOne(d => d.AcademicAuthorizedByNavigation).WithMany(p => p.ProjectsSubmissionAcademicAuthorizedByNavigations)
                .HasForeignKey(d => d.AcademicAuthorizedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_academic_authorized_by_fkey");

            entity.HasOne(d => d.AcademicDocument).WithMany(p => p.ProjectsSubmissions)
                .HasForeignKey(d => d.AcademicDocumentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_academic_document_id_fkey");

            entity.HasOne(d => d.AcademicStatus).WithMany(p => p.ProjectsSubmissions)
                .HasForeignKey(d => d.AcademicStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_academic_status_id_fkey");

            entity.HasOne(d => d.Committee).WithMany(p => p.ProjectsSubmissions)
                .HasForeignKey(d => d.CommitteeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_committee_id_fkey");

            entity.HasOne(d => d.Project).WithOne(p => p.ProjectsSubmission)
                .HasForeignKey<ProjectsSubmission>(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_project_id_fkey");

            entity.HasOne(d => d.SubmittedByNavigation).WithMany(p => p.ProjectsSubmissionSubmittedByNavigations)
                .HasForeignKey(d => d.SubmittedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_submitted_by_fkey");

            entity.HasOne(d => d.ProjectsApplication).WithMany(p => p.ProjectsSubmissions)
                .HasPrincipalKey(p => new { p.Id, p.ProjectId })
                .HasForeignKey(d => new { d.ApplicationId, d.ProjectId })
                .HasConstraintName("projects_submissions_application_id_project_id_fkey");
        });

        modelBuilder.Entity<ResearchOutput>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("research_outputs_pkey");

            entity.ToTable("research_outputs", "research");

            entity.HasIndex(e => e.ProjectId, "fk_156669_idx");

            entity.HasIndex(e => e.OutputTypeId, "fk_156674_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BibliographicCitation).HasColumnName("bibliographic_citation");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Doi)
                .HasMaxLength(250)
                .HasColumnName("doi");
            entity.Property(e => e.ExternalUrl).HasColumnName("external_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OutputTypeId).HasColumnName("output_type_id");
            entity.Property(e => e.ProducedOn).HasColumnName("produced_on");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.OutputType).WithMany(p => p.ResearchOutputs)
                .HasForeignKey(d => d.OutputTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_output_type_id_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.ResearchOutputs)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_project_id_fkey");
        });

        modelBuilder.Entity<ResearchOutputsDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("research_outputs_documents_pkey");

            entity.ToTable("research_outputs_documents", "research");

            entity.HasIndex(e => e.ResearchOutputId, "fk_156743_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156748_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ResearchOutputId).HasColumnName("research_output_id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.ResearchOutput).WithMany(p => p.ResearchOutputsDocuments)
                .HasForeignKey(d => d.ResearchOutputId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_documents_research_output_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.ResearchOutputsDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("research_outputs_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<ResearchOutputsResearcher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("research_outputs_researchers_pkey");

            entity.ToTable("research_outputs_researchers", "research");

            entity.HasIndex(e => e.ResearchOutputId, "fk_156704_idx");

            entity.HasIndex(e => e.ResearcherId, "fk_156709_idx");

            entity.HasIndex(e => new { e.ResearchOutputId, e.AuthorOrder }, "research_outputs_researchers_author_order_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.ResearchOutputId, e.ResearcherId }, "research_outputs_researchers_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorOrder).HasColumnName("author_order");
            entity.Property(e => e.Contribution).HasColumnName("contribution");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCorrespondingAuthor).HasColumnName("is_corresponding_author");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ResearchOutputId).HasColumnName("research_output_id");
            entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ResearchOutput).WithMany(p => p.ResearchOutputsResearchers)
                .HasForeignKey(d => d.ResearchOutputId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_researchers_research_output_id_fkey");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ResearchOutputsResearchers)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_researchers_researcher_id_fkey");
        });

        modelBuilder.Entity<Researcher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("researchers_pkey");

            entity.ToTable("researchers", "academic");

            entity.HasIndex(e => e.UserId, "fk_155113_idx");

            entity.HasIndex(e => e.Orcid, "researchers_orcid_key").IsUnique();

            entity.HasIndex(e => e.UserId, "researchers_user_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Orcid)
                .HasMaxLength(19)
                .HasColumnName("orcid");
            entity.Property(e => e.ProfessionalSummary).HasColumnName("professional_summary");
            entity.Property(e => e.ResearchInterests).HasColumnName("research_interests");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Researcher)
                .HasForeignKey<Researcher>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_user_id_fkey");
        });

        modelBuilder.Entity<ResearchersDegree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("researchers_degrees_pkey");

            entity.ToTable("researchers_degrees", "academic");

            entity.HasIndex(e => e.ResearcherId, "fk_155175_idx");

            entity.HasIndex(e => e.DegreeLevelId, "fk_155180_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DegreeLevelId).HasColumnName("degree_level_id");
            entity.Property(e => e.DegreeTitle)
                .HasMaxLength(300)
                .HasColumnName("degree_title");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.GraduationYear).HasColumnName("graduation_year");
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(300)
                .HasColumnName("institution_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProfessionalLicense)
                .HasMaxLength(100)
                .HasColumnName("professional_license");
            entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.DegreeLevel).WithMany(p => p.ResearchersDegrees)
                .HasForeignKey(d => d.DegreeLevelId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_degrees_degree_level_id_fkey");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ResearchersDegrees)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_degrees_researcher_id_fkey");
        });

        modelBuilder.Entity<ResearchersExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("researchers_experiences_pkey");

            entity.ToTable("researchers_experiences", "academic");

            entity.HasIndex(e => e.ResearcherId, "fk_155209_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrganizationName)
                .HasMaxLength(300)
                .HasColumnName("organization_name");
            entity.Property(e => e.PositionTitle)
                .HasMaxLength(250)
                .HasColumnName("position_title");
            entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ResearchersExperiences)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_experiences_researcher_id_fkey");
        });

        modelBuilder.Entity<ResearchersOrganization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("researchers_organizations_pkey");

            entity.ToTable("researchers_organizations", "academic");

            entity.HasIndex(e => e.ResearcherId, "fk_155141_idx");

            entity.HasIndex(e => e.OrganizationId, "fk_155146_idx");

            entity.HasIndex(e => new { e.ResearcherId, e.OrganizationId }, "researchers_organizations_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EmployeeNumber)
                .HasMaxLength(80)
                .HasColumnName("employee_number");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
            entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Organization).WithMany(p => p.ResearchersOrganizations)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_organizations_organization_id_fkey");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ResearchersOrganizations)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_organizations_researcher_id_fkey");
        });

        modelBuilder.Entity<ResearchersResearchLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("researchers_research_lines_pkey");

            entity.ToTable("researchers_research_lines", "academic");

            entity.HasIndex(e => e.ResearcherId, "fk_155235_idx");

            entity.HasIndex(e => e.ResearchLineId, "fk_155240_idx");

            entity.HasIndex(e => new { e.ResearcherId, e.ResearchLineId }, "researchers_research_lines_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ResearchLineId).HasColumnName("research_line_id");
            entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ResearchLine).WithMany(p => p.ResearchersResearchLines)
                .HasForeignKey(d => d.ResearchLineId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_research_lines_research_line_id_fkey");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ResearchersResearchLines)
                .HasForeignKey(d => d.ResearcherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_research_lines_researcher_id_fkey");
        });

        modelBuilder.Entity<RolesPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_permissions_pkey");

            entity.ToTable("roles_permissions", "access");

            entity.HasIndex(e => e.RoleId, "fk_154910_idx");

            entity.HasIndex(e => e.PermissionId, "fk_154915_idx");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "roles_permissions_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolesPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("roles_permissions_permission_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.RolesPermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("roles_permissions_role_id_fkey");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("students_pkey");

            entity.ToTable("students", "academic");

            entity.HasIndex(e => e.UserId, "fk_155270_idx");

            entity.HasIndex(e => e.AcademicProgramId, "fk_155275_idx");

            entity.HasIndex(e => new { e.AcademicProgramId, e.StudentNumber }, "students_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.UserId, "students_user_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicProgramId).HasColumnName("academic_program_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.StudentNumber)
                .HasMaxLength(80)
                .HasColumnName("student_number");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.AcademicProgram).WithMany(p => p.Students)
                .HasForeignKey(d => d.AcademicProgramId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("students_academic_program_id_fkey");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("students_user_id_fkey");
        });

        modelBuilder.Entity<SubmissionsDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("submissions_documents_pkey");

            entity.ToTable("submissions_documents", "reviews");

            entity.HasIndex(e => e.SubmissionId, "fk_157113_idx");

            entity.HasIndex(e => e.ProjectDocumentId, "fk_157118_idx");

            entity.HasIndex(e => new { e.SubmissionId, e.ProjectDocumentId }, "submissions_documents_submission_id_project_document_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProjectDocumentId).HasColumnName("project_document_id");
            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ProjectDocument).WithMany(p => p.SubmissionsDocuments)
                .HasForeignKey(d => d.ProjectDocumentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("submissions_documents_project_document_id_fkey");

            entity.HasOne(d => d.Submission).WithMany(p => p.SubmissionsDocuments)
                .HasForeignKey(d => d.SubmissionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("submissions_documents_submission_id_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pkey");

            entity.ToTable("tasks", "research");

            entity.HasIndex(e => e.MilestoneId, "fk_155999_idx");

            entity.HasIndex(e => e.ResponsibleParticipantId, "fk_156004_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndsOn).HasColumnName("ends_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.MilestoneId).HasColumnName("milestone_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ResponsibleParticipantId).HasColumnName("responsible_participant_id");
            entity.Property(e => e.StartsOn).HasColumnName("starts_on");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Milestone).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.MilestoneId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tasks_milestone_id_fkey");

            entity.HasOne(d => d.ResponsibleParticipant).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ResponsibleParticipantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tasks_responsible_participant_id_fkey");
        });

        modelBuilder.Entity<TasksCurrentStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("tasks_current_status", "research");

            entity.Property(e => e.ChangedAt).HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CompletionPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("completion_percentage");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsVisible).HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<TasksStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_status_history_pkey");

            entity.ToTable("tasks_status_history", "research");

            entity.HasIndex(e => e.TaskId, "fk_156241_idx");

            entity.HasIndex(e => e.StatusId, "fk_156246_idx");

            entity.HasIndex(e => e.ChangedBy, "fk_156251_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.CompletionPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("completion_percentage");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.TasksStatusHistories)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tasks_status_history_changed_by_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.TasksStatusHistories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tasks_status_history_status_id_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.TasksStatusHistories)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tasks_status_history_task_id_fkey");
        });

        modelBuilder.Entity<TasksStatusHistoryDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_status_history_documents_pkey");

            entity.ToTable("tasks_status_history_documents", "research");

            entity.HasIndex(e => e.StatusHistoryId, "fk_156285_idx");

            entity.HasIndex(e => new { e.DocumentId, e.DocumentTypeId }, "fk_156290_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StatusHistoryId).HasColumnName("status_history_id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionNumber)
                .HasDefaultValue(1)
                .HasColumnName("version_number");

            entity.HasOne(d => d.StatusHistory).WithMany(p => p.TasksStatusHistoryDocuments)
                .HasForeignKey(d => d.StatusHistoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tasks_status_history_documents_status_history_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.TasksStatusHistoryDocuments)
                .HasPrincipalKey(p => new { p.Id, p.DocumentTypeId })
                .HasForeignKey(d => new { d.DocumentId, d.DocumentTypeId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tasks_status_history_document_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", "access");

            entity.HasIndex(e => new { e.IdentityProvider, e.ExternalSubject }, "users_identity_provider_external_subject_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.ExternalSubject)
                .HasMaxLength(250)
                .HasColumnName("external_subject");
            entity.Property(e => e.FamilyName)
                .HasMaxLength(200)
                .HasColumnName("family_name");
            entity.Property(e => e.GivenName)
                .HasMaxLength(150)
                .HasColumnName("given_name");
            entity.Property(e => e.IdentityProvider)
                .HasMaxLength(100)
                .HasColumnName("identity_provider");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<UsersRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_roles_pkey");

            entity.ToTable("users_roles", "access");

            entity.HasIndex(e => e.UserId, "fk_154879_idx");

            entity.HasIndex(e => e.RoleId, "fk_154884_idx");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "users_roles_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_roles_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_roles_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
