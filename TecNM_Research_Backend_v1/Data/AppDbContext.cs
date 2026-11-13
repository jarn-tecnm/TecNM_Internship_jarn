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

    public virtual DbSet<academic_authorizations_history> academic_authorizations_history { get; set; }

    public virtual DbSet<academic_groups> academic_groups { get; set; }

    public virtual DbSet<academic_groups_researchers> academic_groups_researchers { get; set; }

    public virtual DbSet<academic_groups_status_history> academic_groups_status_history { get; set; }

    public virtual DbSet<activities> activities { get; set; }

    public virtual DbSet<activities_current_status> activities_current_status { get; set; }

    public virtual DbSet<activities_status_history> activities_status_history { get; set; }

    public virtual DbSet<activities_status_history_documents> activities_status_history_documents { get; set; }

    public virtual DbSet<audit_logs> audit_logs { get; set; }

    public virtual DbSet<c_academic_authorization_statuses> c_academic_authorization_statuses { get; set; }

    public virtual DbSet<c_academic_fields> c_academic_fields { get; set; }

    public virtual DbSet<c_academic_group_levels> c_academic_group_levels { get; set; }

    public virtual DbSet<c_academic_programs> c_academic_programs { get; set; }

    public virtual DbSet<c_activity_statuses> c_activity_statuses { get; set; }

    public virtual DbSet<c_budget_categories> c_budget_categories { get; set; }

    public virtual DbSet<c_call_types> c_call_types { get; set; }

    public virtual DbSet<c_calls> c_calls { get; set; }

    public virtual DbSet<c_committee_roles> c_committee_roles { get; set; }

    public virtual DbSet<c_currencies> c_currencies { get; set; }

    public virtual DbSet<c_degree_levels> c_degree_levels { get; set; }

    public virtual DbSet<c_document_templates> c_document_templates { get; set; }

    public virtual DbSet<c_document_types> c_document_types { get; set; }

    public virtual DbSet<c_funding_sources> c_funding_sources { get; set; }

    public virtual DbSet<c_milestone_statuses> c_milestone_statuses { get; set; }

    public virtual DbSet<c_objective_statuses> c_objective_statuses { get; set; }

    public virtual DbSet<c_organization_types> c_organization_types { get; set; }

    public virtual DbSet<c_organizations> c_organizations { get; set; }

    public virtual DbSet<c_permissions> c_permissions { get; set; }

    public virtual DbSet<c_project_statuses> c_project_statuses { get; set; }

    public virtual DbSet<c_research_lines> c_research_lines { get; set; }

    public virtual DbSet<c_research_outputs_types> c_research_outputs_types { get; set; }

    public virtual DbSet<c_researchers_roles> c_researchers_roles { get; set; }

    public virtual DbSet<c_review_outcomes> c_review_outcomes { get; set; }

    public virtual DbSet<c_review_statuses> c_review_statuses { get; set; }

    public virtual DbSet<c_roles> c_roles { get; set; }

    public virtual DbSet<calls_document_requirements> calls_document_requirements { get; set; }

    public virtual DbSet<calls_requirements> calls_requirements { get; set; }

    public virtual DbSet<committees> committees { get; set; }

    public virtual DbSet<committees_members> committees_members { get; set; }

    public virtual DbSet<documents> documents { get; set; }

    public virtual DbSet<milestones> milestones { get; set; }

    public virtual DbSet<milestones_current_status> milestones_current_status { get; set; }

    public virtual DbSet<milestones_status_history> milestones_status_history { get; set; }

    public virtual DbSet<milestones_status_history_documents> milestones_status_history_documents { get; set; }

    public virtual DbSet<objectives> objectives { get; set; }

    public virtual DbSet<objectives_current_status> objectives_current_status { get; set; }

    public virtual DbSet<objectives_status_history> objectives_status_history { get; set; }

    public virtual DbSet<objectives_status_history_documents> objectives_status_history_documents { get; set; }

    public virtual DbSet<projects> projects { get; set; }

    public virtual DbSet<projects_academic_groups> projects_academic_groups { get; set; }

    public virtual DbSet<projects_applications> projects_applications { get; set; }

    public virtual DbSet<projects_applications_documents> projects_applications_documents { get; set; }

    public virtual DbSet<projects_budget_documents> projects_budget_documents { get; set; }

    public virtual DbSet<projects_budget_items> projects_budget_items { get; set; }

    public virtual DbSet<projects_current_status> projects_current_status { get; set; }

    public virtual DbSet<projects_documents> projects_documents { get; set; }

    public virtual DbSet<projects_expenses> projects_expenses { get; set; }

    public virtual DbSet<projects_expenses_documents> projects_expenses_documents { get; set; }

    public virtual DbSet<projects_funding> projects_funding { get; set; }

    public virtual DbSet<projects_funding_documents> projects_funding_documents { get; set; }

    public virtual DbSet<projects_leads> projects_leads { get; set; }

    public virtual DbSet<projects_research_lines> projects_research_lines { get; set; }

    public virtual DbSet<projects_researchers> projects_researchers { get; set; }

    public virtual DbSet<projects_reviews> projects_reviews { get; set; }

    public virtual DbSet<projects_status_history> projects_status_history { get; set; }

    public virtual DbSet<projects_students> projects_students { get; set; }

    public virtual DbSet<projects_submissions> projects_submissions { get; set; }

    public virtual DbSet<research_outputs> research_outputs { get; set; }

    public virtual DbSet<research_outputs_documents> research_outputs_documents { get; set; }

    public virtual DbSet<research_outputs_researchers> research_outputs_researchers { get; set; }

    public virtual DbSet<researchers> researchers { get; set; }

    public virtual DbSet<researchers_degrees> researchers_degrees { get; set; }

    public virtual DbSet<researchers_experiences> researchers_experiences { get; set; }

    public virtual DbSet<researchers_organizations> researchers_organizations { get; set; }

    public virtual DbSet<researchers_research_lines> researchers_research_lines { get; set; }

    public virtual DbSet<roles_permissions> roles_permissions { get; set; }

    public virtual DbSet<students> students { get; set; }

    public virtual DbSet<submissions_documents> submissions_documents { get; set; }

    public virtual DbSet<users> users { get; set; }

    public virtual DbSet<users_roles> users_roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<academic_authorizations_history>(entity =>
        {
            entity.HasKey(e => e.id).HasName("academic_authorizations_history_pkey");

            entity.ToTable("academic_authorizations_history", "reviews");

            entity.HasIndex(e => new { e.submission_id, e.decided_at, e.id }, "academic_authorizations_history_latest_idx").IsDescending(false, true, true);

            entity.HasIndex(e => e.submission_id, "fk_169845_idx");

            entity.HasIndex(e => e.status_id, "fk_169850_idx");

            entity.HasIndex(e => e.authorized_by, "fk_169855_idx");

            entity.HasIndex(e => e.document_id, "fk_169860_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.authorized_byNavigation).WithMany(p => p.academic_authorizations_history)
                .HasForeignKey(d => d.authorized_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_authorized_by_fkey");

            entity.HasOne(d => d.document).WithMany(p => p.academic_authorizations_history)
                .HasForeignKey(d => d.document_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_document_id_fkey");

            entity.HasOne(d => d.status).WithMany(p => p.academic_authorizations_history)
                .HasForeignKey(d => d.status_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_status_id_fkey");

            entity.HasOne(d => d.submission).WithMany(p => p.academic_authorizations_history)
                .HasForeignKey(d => d.submission_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_authorizations_history_submission_id_fkey");
        });

        modelBuilder.Entity<academic_groups>(entity =>
        {
            entity.HasKey(e => e.id).HasName("academic_groups_pkey");

            entity.ToTable("academic_groups", "academic");

            entity.HasIndex(e => e.code, "academic_groups_code_key").IsUnique();

            entity.HasIndex(e => e.organization_id, "fk_169988_idx");

            entity.Property(e => e.code).HasMaxLength(100);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.organization).WithMany(p => p.academic_groups)
                .HasForeignKey(d => d.organization_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_groups_organization_id_fkey");
        });

        modelBuilder.Entity<academic_groups_researchers>(entity =>
        {
            entity.HasKey(e => e.id).HasName("academic_groups_researchers_pkey");

            entity.ToTable("academic_groups_researchers", "academic");

            entity.HasIndex(e => new { e.academic_group_id, e.researcher_id, e.starts_on }, "academic_groups_researchers_academic_group_id_researcher_id_key").IsUnique();

            entity.HasIndex(e => new { e.academic_group_id, e.researcher_id }, "academic_groups_researchers_current_uq")
                .IsUnique()
                .HasFilter("((ends_on IS NULL) AND is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.academic_group_id, "academic_groups_researchers_leader_uq")
                .IsUnique()
                .HasFilter("(is_leader AND (ends_on IS NULL) AND is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.academic_group_id, "fk_170020_idx");

            entity.HasIndex(e => e.researcher_id, "fk_170025_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.academic_group).WithOne(p => p.academic_groups_researchers)
                .HasForeignKey<academic_groups_researchers>(d => d.academic_group_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_groups_researchers_academic_group_id_fkey");

            entity.HasOne(d => d.researcher).WithMany(p => p.academic_groups_researchers)
                .HasForeignKey(d => d.researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_groups_researchers_researcher_id_fkey");
        });

        modelBuilder.Entity<academic_groups_status_history>(entity =>
        {
            entity.HasKey(e => e.id).HasName("academic_groups_status_history_pkey");

            entity.ToTable("academic_groups_status_history", "academic");

            entity.HasIndex(e => new { e.academic_group_id, e.changed_at, e.id }, "academic_groups_status_history_latest_idx").IsDescending(false, true, true);

            entity.HasIndex(e => e.academic_group_id, "fk_170056_idx");

            entity.HasIndex(e => e.level_id, "fk_170061_idx");

            entity.HasIndex(e => e.changed_by, "fk_170066_idx");

            entity.Property(e => e.changed_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.academic_group).WithMany(p => p.academic_groups_status_history)
                .HasForeignKey(d => d.academic_group_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_groups_status_history_academic_group_id_fkey");

            entity.HasOne(d => d.changed_byNavigation).WithMany(p => p.academic_groups_status_history)
                .HasForeignKey(d => d.changed_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_groups_status_history_changed_by_fkey");

            entity.HasOne(d => d.level).WithMany(p => p.academic_groups_status_history)
                .HasForeignKey(d => d.level_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("academic_groups_status_history_level_id_fkey");
        });

        modelBuilder.Entity<activities>(entity =>
        {
            entity.HasKey(e => e.id).HasName("activities_pkey");

            entity.ToTable("activities", "research");

            entity.HasIndex(e => e.milestone_id, "fk_168766_idx");

            entity.HasIndex(e => e.responsible_participant_id, "fk_168771_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.milestone).WithMany(p => p.activities)
                .HasForeignKey(d => d.milestone_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("activities_milestone_id_fkey");

            entity.HasOne(d => d.responsible_participant).WithMany(p => p.activities)
                .HasForeignKey(d => d.responsible_participant_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("activities_responsible_participant_id_fkey");
        });

        modelBuilder.Entity<activities_current_status>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("activities_current_status", "research");

            entity.Property(e => e.completion_percentage).HasPrecision(5, 2);
        });

        modelBuilder.Entity<activities_status_history>(entity =>
        {
            entity.HasKey(e => e.id).HasName("activities_status_history_pkey");

            entity.ToTable("activities_status_history", "research");

            entity.HasIndex(e => e.activity_id, "fk_169008_idx");

            entity.HasIndex(e => e.status_id, "fk_169013_idx");

            entity.HasIndex(e => e.changed_by, "fk_169018_idx");

            entity.Property(e => e.changed_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.completion_percentage).HasPrecision(5, 2);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.activity).WithMany(p => p.activities_status_history)
                .HasForeignKey(d => d.activity_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("activities_status_history_activity_id_fkey");

            entity.HasOne(d => d.changed_byNavigation).WithMany(p => p.activities_status_history)
                .HasForeignKey(d => d.changed_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("activities_status_history_changed_by_fkey");

            entity.HasOne(d => d.status).WithMany(p => p.activities_status_history)
                .HasForeignKey(d => d.status_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("activities_status_history_status_id_fkey");
        });

        modelBuilder.Entity<activities_status_history_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("activities_status_history_documents_pkey");

            entity.ToTable("activities_status_history_documents", "research");

            entity.HasIndex(e => e.status_history_id, "fk_169052_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_169057_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.status_history).WithMany(p => p.activities_status_history_documents)
                .HasForeignKey(d => d.status_history_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("activities_status_history_documents_status_history_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.activities_status_history_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activities_status_history_doc_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<audit_logs>(entity =>
        {
            entity.HasKey(e => e.id).HasName("audit_logs_pkey");

            entity.ToTable("audit_logs", "access");

            entity.HasIndex(e => e.actor_user_id, "fk_169891_idx");

            entity.HasIndex(e => e.project_id, "fk_169896_idx");

            entity.Property(e => e.action).HasMaxLength(100);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.details)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb");
            entity.Property(e => e.entity_schema).HasMaxLength(63);
            entity.Property(e => e.entity_table).HasMaxLength(63);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.occurred_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.request_id).HasMaxLength(100);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.actor_user).WithMany(p => p.audit_logs)
                .HasForeignKey(d => d.actor_user_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("audit_logs_actor_user_id_fkey");

            entity.HasOne(d => d.project).WithMany(p => p.audit_logs)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("audit_logs_project_id_fkey");
        });

        modelBuilder.Entity<c_academic_authorization_statuses>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_academic_authorization_statuses_pkey");

            entity.ToTable("c_academic_authorization_statuses", "reviews");

            entity.HasIndex(e => e.code, "c_academic_authorization_statuses_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_academic_fields>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_academic_fields_pkey");

            entity.ToTable("c_academic_fields", "academic");

            entity.HasIndex(e => e.code, "c_academic_fields_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_academic_group_levels>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_academic_group_levels_pkey");

            entity.ToTable("c_academic_group_levels", "academic");

            entity.HasIndex(e => e.code, "c_academic_group_levels_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_academic_programs>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_academic_programs_pkey");

            entity.ToTable("c_academic_programs", "academic");

            entity.HasIndex(e => e.code, "c_academic_programs_code_key").IsUnique();

            entity.HasIndex(e => e.organization_id, "fk_167762_idx");

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.organization).WithMany(p => p.c_academic_programs)
                .HasForeignKey(d => d.organization_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_academic_programs_organization_id_fkey");
        });

        modelBuilder.Entity<c_activity_statuses>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_activity_statuses_pkey");

            entity.ToTable("c_activity_statuses", "research");

            entity.HasIndex(e => e.code, "c_activity_statuses_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_budget_categories>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_budget_categories_pkey");

            entity.ToTable("c_budget_categories", "research");

            entity.HasIndex(e => e.code, "c_budget_categories_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_call_types>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_call_types_pkey");

            entity.ToTable("c_call_types", "calls");

            entity.HasIndex(e => e.code, "c_call_types_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_calls>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_calls_pkey");

            entity.ToTable("c_calls", "calls");

            entity.HasIndex(e => e.code, "c_calls_code_key").IsUnique();

            entity.HasIndex(e => e.organization_id, "fk_168094_idx");

            entity.HasIndex(e => e.call_type_id, "fk_168099_idx");

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.call_type).WithMany(p => p.c_calls)
                .HasForeignKey(d => d.call_type_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_calls_call_type_id_fkey");

            entity.HasOne(d => d.organization).WithMany(p => p.c_calls)
                .HasForeignKey(d => d.organization_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_calls_organization_id_fkey");
        });

        modelBuilder.Entity<c_committee_roles>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_committee_roles_pkey");

            entity.ToTable("c_committee_roles", "reviews");

            entity.HasIndex(e => e.code, "c_committee_roles_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_currencies>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_currencies_pkey");

            entity.ToTable("c_currencies", "research");

            entity.HasIndex(e => e.code, "c_currencies_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_degree_levels>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_degree_levels_pkey");

            entity.ToTable("c_degree_levels", "academic");

            entity.HasIndex(e => e.code, "c_degree_levels_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_document_templates>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_document_templates_pkey");

            entity.ToTable("c_document_templates", "files");

            entity.HasIndex(e => new { e.code, e.version_number }, "c_document_templates_code_version_number_key").IsUnique();

            entity.HasIndex(e => e.code, "c_document_templates_current_uq")
                .IsUnique()
                .HasFilter("(is_current AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.storage_key, "c_document_templates_storage_key_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.media_type).HasMaxLength(150);
            entity.Property(e => e.name).HasMaxLength(300);
            entity.Property(e => e.original_file_name).HasMaxLength(300);
            entity.Property(e => e.sha256)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_document_types>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_document_types_pkey");

            entity.ToTable("c_document_types", "files");

            entity.HasIndex(e => e.code, "c_document_types_code_key").IsUnique();

            entity.HasIndex(e => e.template_id, "fk_168163_idx");

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.template).WithMany(p => p.c_document_types)
                .HasForeignKey(d => d.template_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_document_types_template_id_fkey");
        });

        modelBuilder.Entity<c_funding_sources>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_funding_sources_pkey");

            entity.ToTable("c_funding_sources", "research");

            entity.HasIndex(e => e.code, "c_funding_sources_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_milestone_statuses>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_milestone_statuses_pkey");

            entity.ToTable("c_milestone_statuses", "research");

            entity.HasIndex(e => e.code, "c_milestone_statuses_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_objective_statuses>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_objective_statuses_pkey");

            entity.ToTable("c_objective_statuses", "research");

            entity.HasIndex(e => e.code, "c_objective_statuses_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_organization_types>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_organization_types_pkey");

            entity.ToTable("c_organization_types", "academic");

            entity.HasIndex(e => e.code, "c_organization_types_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_organizations>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_organizations_pkey");

            entity.ToTable("c_organizations", "academic");

            entity.HasIndex(e => e.code, "c_organizations_code_key").IsUnique();

            entity.HasIndex(e => e.organization_type_id, "fk_167731_idx");

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.country_code)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.organization_type).WithMany(p => p.c_organizations)
                .HasForeignKey(d => d.organization_type_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_organizations_organization_type_id_fkey");
        });

        modelBuilder.Entity<c_permissions>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_permissions_pkey");

            entity.ToTable("c_permissions", "access");

            entity.HasIndex(e => e.code, "c_permissions_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_project_statuses>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_project_statuses_pkey");

            entity.ToTable("c_project_statuses", "research");

            entity.HasIndex(e => e.code, "c_project_statuses_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_research_lines>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_research_lines_pkey");

            entity.ToTable("c_research_lines", "academic");

            entity.HasIndex(e => e.code, "c_research_lines_code_key").IsUnique();

            entity.HasIndex(e => e.academic_field_id, "fk_167818_idx");

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.academic_field).WithMany(p => p.c_research_lines)
                .HasForeignKey(d => d.academic_field_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("c_research_lines_academic_field_id_fkey");
        });

        modelBuilder.Entity<c_research_outputs_types>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_research_outputs_types_pkey");

            entity.ToTable("c_research_outputs_types", "research");

            entity.HasIndex(e => e.code, "c_research_outputs_types_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_researchers_roles>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_researchers_roles_pkey");

            entity.ToTable("c_researchers_roles", "research");

            entity.HasIndex(e => e.code, "c_researchers_roles_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_review_outcomes>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_review_outcomes_pkey");

            entity.ToTable("c_review_outcomes", "reviews");

            entity.HasIndex(e => e.code, "c_review_outcomes_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_review_statuses>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_review_statuses_pkey");

            entity.ToTable("c_review_statuses", "reviews");

            entity.HasIndex(e => e.code, "c_review_statuses_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<c_roles>(entity =>
        {
            entity.HasKey(e => e.id).HasName("c_roles_pkey");

            entity.ToTable("c_roles", "access");

            entity.HasIndex(e => e.code, "c_roles_code_key").IsUnique();

            entity.Property(e => e.code).HasMaxLength(80);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<calls_document_requirements>(entity =>
        {
            entity.HasKey(e => e.id).HasName("calls_document_requirements_pkey");

            entity.ToTable("calls_document_requirements", "calls");

            entity.HasIndex(e => new { e.call_id, e.document_type_id, e.title, e.workflow_stage }, "calls_document_requirements_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.call_id, "fk_168238_idx");

            entity.HasIndex(e => e.document_type_id, "fk_168243_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_required).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.workflow_stage).HasMaxLength(40);

            entity.HasOne(d => d.call).WithMany(p => p.calls_document_requirements)
                .HasForeignKey(d => d.call_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("calls_document_requirements_call_id_fkey");

            entity.HasOne(d => d.document_type).WithMany(p => p.calls_document_requirements)
                .HasForeignKey(d => d.document_type_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("calls_document_requirements_document_type_id_fkey");
        });

        modelBuilder.Entity<calls_requirements>(entity =>
        {
            entity.HasKey(e => e.id).HasName("calls_requirements_pkey");

            entity.ToTable("calls_requirements", "calls");

            entity.HasIndex(e => e.call_id, "fk_168272_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_required).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.call).WithMany(p => p.calls_requirements)
                .HasForeignKey(d => d.call_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("calls_requirements_call_id_fkey");
        });

        modelBuilder.Entity<committees>(entity =>
        {
            entity.HasKey(e => e.id).HasName("committees_pkey");

            entity.ToTable("committees", "reviews");

            entity.HasIndex(e => e.academic_program_id, "fk_169640_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.academic_program).WithMany(p => p.committees)
                .HasForeignKey(d => d.academic_program_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_academic_program_id_fkey");
        });

        modelBuilder.Entity<committees_members>(entity =>
        {
            entity.HasKey(e => e.id).HasName("committees_members_pkey");

            entity.ToTable("committees_members", "reviews");

            entity.HasIndex(e => new { e.id, e.committee_id }, "committees_members_id_committee_id_key").IsUnique();

            entity.HasIndex(e => new { e.committee_id, e.user_id }, "committees_members_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.committee_id, e.committee_role_id }, "committees_members_role_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.committee_id, "fk_169671_idx");

            entity.HasIndex(e => e.user_id, "fk_169676_idx");

            entity.HasIndex(e => e.committee_role_id, "fk_169681_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.committee).WithMany(p => p.committees_members)
                .HasForeignKey(d => d.committee_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_members_committee_id_fkey");

            entity.HasOne(d => d.committee_role).WithMany(p => p.committees_members)
                .HasForeignKey(d => d.committee_role_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_members_committee_role_id_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.committees_members)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("committees_members_user_id_fkey");
        });

        modelBuilder.Entity<documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("documents_pkey");

            entity.ToTable("documents", "files");

            entity.HasIndex(e => new { e.id, e.document_type_id }, "documents_id_document_type_id_key").IsUnique();

            entity.HasIndex(e => e.storage_key, "documents_storage_key_key").IsUnique();

            entity.HasIndex(e => e.document_type_id, "fk_168202_idx");

            entity.HasIndex(e => e.uploaded_by, "fk_168207_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.media_type).HasMaxLength(150);
            entity.Property(e => e.original_file_name).HasMaxLength(300);
            entity.Property(e => e.sha256)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.uploaded_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.document_type).WithMany(p => p.documents)
                .HasForeignKey(d => d.document_type_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("documents_document_type_id_fkey");

            entity.HasOne(d => d.uploaded_byNavigation).WithMany(p => p.documents)
                .HasForeignKey(d => d.uploaded_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("documents_uploaded_by_fkey");
        });

        modelBuilder.Entity<milestones>(entity =>
        {
            entity.HasKey(e => e.id).HasName("milestones_pkey");

            entity.ToTable("milestones", "research");

            entity.HasIndex(e => e.objective_id, "fk_168737_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.objective).WithMany(p => p.milestones)
                .HasForeignKey(d => d.objective_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_objective_id_fkey");
        });

        modelBuilder.Entity<milestones_current_status>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("milestones_current_status", "research");

            entity.Property(e => e.completion_percentage).HasPrecision(5, 2);
        });

        modelBuilder.Entity<milestones_status_history>(entity =>
        {
            entity.HasKey(e => e.id).HasName("milestones_status_history_pkey");

            entity.ToTable("milestones_status_history", "research");

            entity.HasIndex(e => e.milestone_id, "fk_168926_idx");

            entity.HasIndex(e => e.status_id, "fk_168931_idx");

            entity.HasIndex(e => e.changed_by, "fk_168936_idx");

            entity.Property(e => e.changed_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.completion_percentage).HasPrecision(5, 2);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.changed_byNavigation).WithMany(p => p.milestones_status_history)
                .HasForeignKey(d => d.changed_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_changed_by_fkey");

            entity.HasOne(d => d.milestone).WithMany(p => p.milestones_status_history)
                .HasForeignKey(d => d.milestone_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_milestone_id_fkey");

            entity.HasOne(d => d.status).WithMany(p => p.milestones_status_history)
                .HasForeignKey(d => d.status_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_status_id_fkey");
        });

        modelBuilder.Entity<milestones_status_history_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("milestones_status_history_documents_pkey");

            entity.ToTable("milestones_status_history_documents", "research");

            entity.HasIndex(e => e.status_history_id, "fk_168970_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_168975_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.status_history).WithMany(p => p.milestones_status_history_documents)
                .HasForeignKey(d => d.status_history_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("milestones_status_history_documents_status_history_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.milestones_status_history_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("milestones_status_history_doc_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<objectives>(entity =>
        {
            entity.HasKey(e => e.id).HasName("objectives_pkey");

            entity.ToTable("objectives", "research");

            entity.HasIndex(e => e.project_id, "fk_168710_idx");

            entity.HasIndex(e => e.project_id, "objectives_general_uq")
                .IsUnique()
                .HasFilter("(((objective_type)::text = 'general'::text) AND is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.id, e.project_id }, "objectives_id_project_id_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.objective_type).HasMaxLength(10);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.project).WithOne(p => p.objectives)
                .HasForeignKey<objectives>(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_project_id_fkey");
        });

        modelBuilder.Entity<objectives_current_status>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("objectives_current_status", "research");

            entity.Property(e => e.completion_percentage).HasPrecision(5, 2);
        });

        modelBuilder.Entity<objectives_status_history>(entity =>
        {
            entity.HasKey(e => e.id).HasName("objectives_status_history_pkey");

            entity.ToTable("objectives_status_history", "research");

            entity.HasIndex(e => e.objective_id, "fk_168844_idx");

            entity.HasIndex(e => e.status_id, "fk_168849_idx");

            entity.HasIndex(e => e.changed_by, "fk_168854_idx");

            entity.Property(e => e.changed_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.completion_percentage).HasPrecision(5, 2);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.changed_byNavigation).WithMany(p => p.objectives_status_history)
                .HasForeignKey(d => d.changed_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_changed_by_fkey");

            entity.HasOne(d => d.objective).WithMany(p => p.objectives_status_history)
                .HasForeignKey(d => d.objective_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_objective_id_fkey");

            entity.HasOne(d => d.status).WithMany(p => p.objectives_status_history)
                .HasForeignKey(d => d.status_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_status_id_fkey");
        });

        modelBuilder.Entity<objectives_status_history_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("objectives_status_history_documents_pkey");

            entity.ToTable("objectives_status_history_documents", "research");

            entity.HasIndex(e => e.status_history_id, "fk_168888_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_168893_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.status_history).WithMany(p => p.objectives_status_history_documents)
                .HasForeignKey(d => d.status_history_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("objectives_status_history_documents_status_history_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.objectives_status_history_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("objectives_status_history_doc_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<projects>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_pkey");

            entity.ToTable("projects", "research");

            entity.HasIndex(e => e.organization_id, "fk_168539_idx");

            entity.HasIndex(e => e.currency_id, "fk_168544_idx");

            entity.HasIndex(e => e.reference_code, "projects_reference_code_key").IsUnique();

            entity.Property(e => e._abstract).HasColumnName("abstract");
            entity.Property(e => e.activities_progress_percentage).HasPrecision(5, 2);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_managed_in_platform).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.milestones_progress_percentage).HasPrecision(5, 2);
            entity.Property(e => e.objectives_progress_percentage).HasPrecision(5, 2);
            entity.Property(e => e.reference_code).HasMaxLength(100);
            entity.Property(e => e.title).HasMaxLength(500);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.currency).WithMany(p => p.projects)
                .HasForeignKey(d => d.currency_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_currency_id_fkey");

            entity.HasOne(d => d.organization).WithMany(p => p.projects)
                .HasForeignKey(d => d.organization_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_organization_id_fkey");
        });

        modelBuilder.Entity<projects_academic_groups>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_academic_groups_pkey");

            entity.ToTable("projects_academic_groups", "research");

            entity.HasIndex(e => e.project_id, "fk_170092_idx");

            entity.HasIndex(e => e.academic_group_id, "fk_170097_idx");

            entity.HasIndex(e => new { e.project_id, e.academic_group_id }, "projects_academic_groups_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.academic_group).WithMany(p => p.projects_academic_groups)
                .HasForeignKey(d => d.academic_group_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_academic_groups_academic_group_id_fkey");

            entity.HasOne(d => d.project).WithMany(p => p.projects_academic_groups)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_academic_groups_project_id_fkey");
        });

        modelBuilder.Entity<projects_applications>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_applications_pkey");

            entity.ToTable("projects_applications", "research");

            entity.HasIndex(e => e.project_id, "fk_169129_idx");

            entity.HasIndex(e => e.call_id, "fk_169134_idx");

            entity.HasIndex(e => new { e.id, e.project_id }, "projects_applications_id_project_id_key").IsUnique();

            entity.HasIndex(e => new { e.project_id, e.call_id }, "projects_applications_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.application_number).HasMaxLength(100);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.requested_amount).HasPrecision(18, 2);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.call).WithMany(p => p.projects_applications)
                .HasForeignKey(d => d.call_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_applications_call_id_fkey");

            entity.HasOne(d => d.project).WithMany(p => p.projects_applications)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_applications_project_id_fkey");
        });

        modelBuilder.Entity<projects_applications_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_applications_documents_pkey");

            entity.ToTable("projects_applications_documents", "research");

            entity.HasIndex(e => e.application_id, "fk_169168_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_169173_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.application).WithMany(p => p.projects_applications_documents)
                .HasForeignKey(d => d.application_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_applications_documents_application_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.projects_applications_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_applications_documen_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<projects_budget_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_budget_documents_pkey");

            entity.ToTable("projects_budget_documents", "research");

            entity.HasIndex(e => e.budget_item_id, "fk_169246_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_169251_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.budget_item).WithMany(p => p.projects_budget_documents)
                .HasForeignKey(d => d.budget_item_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_budget_documents_budget_item_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.projects_budget_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_budget_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<projects_budget_items>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_budget_items_pkey");

            entity.ToTable("projects_budget_items", "research");

            entity.HasIndex(e => e.project_id, "fk_169207_idx");

            entity.HasIndex(e => e.budget_category_id, "fk_169212_idx");

            entity.HasIndex(e => new { e.id, e.project_id }, "projects_budget_items_id_project_id_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.quantity).HasPrecision(12, 3);
            entity.Property(e => e.total_amount)
                .HasPrecision(24, 2)
                .HasComputedColumnSql("round((quantity * unit_cost), 2)", true);
            entity.Property(e => e.unit_cost).HasPrecision(18, 2);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.budget_category).WithMany(p => p.projects_budget_items)
                .HasForeignKey(d => d.budget_category_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_budget_items_budget_category_id_fkey");

            entity.HasOne(d => d.project).WithMany(p => p.projects_budget_items)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_budget_items_project_id_fkey");
        });

        modelBuilder.Entity<projects_current_status>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("projects_current_status", "research");
        });

        modelBuilder.Entity<projects_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_documents_pkey");

            entity.ToTable("projects_documents", "research");

            entity.HasIndex(e => e.project_id, "fk_169093_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_169098_idx");

            entity.HasIndex(e => new { e.id, e.project_id, e.document_id }, "projects_documents_id_project_id_document_id_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.project).WithMany(p => p.projects_documents)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_documents_project_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.projects_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<projects_expenses>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_expenses_pkey");

            entity.ToTable("projects_expenses", "research");

            entity.HasIndex(e => e.budget_item_id, "fk_169365_idx");

            entity.HasIndex(e => e.funding_id, "fk_169370_idx");

            entity.Property(e => e.amount).HasPrecision(18, 2);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.receipt_number).HasMaxLength(100);
            entity.Property(e => e.supplier_name).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.budget_item).WithMany(p => p.projects_expenses)
                .HasForeignKey(d => d.budget_item_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_expenses_budget_item_id_fkey");

            entity.HasOne(d => d.funding).WithMany(p => p.projects_expenses)
                .HasForeignKey(d => d.funding_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_expenses_funding_id_fkey");
        });

        modelBuilder.Entity<projects_expenses_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_expenses_documents_pkey");

            entity.ToTable("projects_expenses_documents", "research");

            entity.HasIndex(e => e.expense_id, "fk_169404_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_169409_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.expense).WithMany(p => p.projects_expenses_documents)
                .HasForeignKey(d => d.expense_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_expenses_documents_expense_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.projects_expenses_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_expenses_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<projects_funding>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_funding_pkey");

            entity.ToTable("projects_funding", "research");

            entity.HasIndex(e => e.project_id, "fk_169287_idx");

            entity.HasIndex(e => e.funding_source_id, "fk_169292_idx");

            entity.HasIndex(e => new { e.application_id, e.project_id }, "fk_169297_idx");

            entity.HasIndex(e => new { e.id, e.project_id }, "projects_funding_id_project_id_key").IsUnique();

            entity.Property(e => e.approved_amount).HasPrecision(18, 2);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.received_amount).HasPrecision(18, 2);
            entity.Property(e => e.reference_number).HasMaxLength(100);
            entity.Property(e => e.requested_amount).HasPrecision(18, 2);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.funding_source).WithMany(p => p.projects_funding)
                .HasForeignKey(d => d.funding_source_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_funding_funding_source_id_fkey");

            entity.HasOne(d => d.project).WithMany(p => p.projects_funding)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_funding_project_id_fkey");

            entity.HasOne(d => d.projects_applications).WithMany(p => p.projects_funding)
                .HasPrincipalKey(p => new { p.id, p.project_id })
                .HasForeignKey(d => new { d.application_id, d.project_id })
                .HasConstraintName("projects_funding_application_id_project_id_fkey");
        });

        modelBuilder.Entity<projects_funding_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_funding_documents_pkey");

            entity.ToTable("projects_funding_documents", "research");

            entity.HasIndex(e => e.funding_id, "fk_169331_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_169336_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.funding).WithMany(p => p.projects_funding_documents)
                .HasForeignKey(d => d.funding_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_funding_documents_funding_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.projects_funding_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_funding_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<projects_leads>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_leads_pkey");

            entity.ToTable("projects_leads", "research");

            entity.HasIndex(e => e.project_researcher_id, "fk_168616_idx");

            entity.HasIndex(e => e.project_researcher_id, "projects_leads_participant_current_uq")
                .IsUnique()
                .HasFilter("(is_current AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.project_researcher).WithOne(p => p.projects_leads)
                .HasForeignKey<projects_leads>(d => d.project_researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_leads_project_researcher_id_fkey");
        });

        modelBuilder.Entity<projects_research_lines>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_research_lines_pkey");

            entity.ToTable("projects_research_lines", "research");

            entity.HasIndex(e => e.project_id, "fk_168675_idx");

            entity.HasIndex(e => e.research_line_id, "fk_168680_idx");

            entity.HasIndex(e => new { e.project_id, e.research_line_id }, "projects_research_lines_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.project).WithMany(p => p.projects_research_lines)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_research_lines_project_id_fkey");

            entity.HasOne(d => d.research_line).WithMany(p => p.projects_research_lines)
                .HasForeignKey(d => d.research_line_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_research_lines_research_line_id_fkey");
        });

        modelBuilder.Entity<projects_researchers>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_researchers_pkey");

            entity.ToTable("projects_researchers", "research");

            entity.HasIndex(e => e.project_id, "fk_168575_idx");

            entity.HasIndex(e => e.researcher_id, "fk_168580_idx");

            entity.HasIndex(e => e.researcher_role_id, "fk_168585_idx");

            entity.HasIndex(e => new { e.id, e.project_id }, "projects_researchers_id_project_id_key").IsUnique();

            entity.HasIndex(e => new { e.project_id, e.researcher_id }, "projects_researchers_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.project).WithMany(p => p.projects_researchers)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_researchers_project_id_fkey");

            entity.HasOne(d => d.researcher).WithMany(p => p.projects_researchers)
                .HasForeignKey(d => d.researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_researchers_researcher_id_fkey");

            entity.HasOne(d => d.researcher_role).WithMany(p => p.projects_researchers)
                .HasForeignKey(d => d.researcher_role_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_researchers_researcher_role_id_fkey");
        });

        modelBuilder.Entity<projects_reviews>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_reviews_pkey");

            entity.ToTable("projects_reviews", "reviews");

            entity.HasIndex(e => e.status_id, "fk_169799_idx");

            entity.HasIndex(e => e.outcome_id, "fk_169804_idx");

            entity.HasIndex(e => new { e.submission_id, e.committee_id }, "fk_169809_idx");

            entity.HasIndex(e => new { e.committee_member_id, e.committee_id }, "fk_169814_idx");

            entity.HasIndex(e => new { e.submission_id, e.committee_member_id }, "projects_reviews_submission_id_committee_member_id_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.outcome).WithMany(p => p.projects_reviews)
                .HasForeignKey(d => d.outcome_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_reviews_outcome_id_fkey");

            entity.HasOne(d => d.status).WithMany(p => p.projects_reviews)
                .HasForeignKey(d => d.status_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_reviews_status_id_fkey");

            entity.HasOne(d => d.committees_members).WithMany(p => p.projects_reviews)
                .HasPrincipalKey(p => new { p.id, p.committee_id })
                .HasForeignKey(d => new { d.committee_member_id, d.committee_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_reviews_committee_member_id_committee_id_fkey");

            entity.HasOne(d => d.projects_submissions).WithMany(p => p.projects_reviews)
                .HasPrincipalKey(p => new { p.id, p.committee_id })
                .HasForeignKey(d => new { d.submission_id, d.committee_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_reviews_submission_id_committee_id_fkey");
        });

        modelBuilder.Entity<projects_status_history>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_status_history_pkey");

            entity.ToTable("projects_status_history", "research");

            entity.HasIndex(e => e.project_id, "fk_168801_idx");

            entity.HasIndex(e => e.status_id, "fk_168806_idx");

            entity.HasIndex(e => e.changed_by, "fk_168811_idx");

            entity.Property(e => e.changed_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.changed_byNavigation).WithMany(p => p.projects_status_history)
                .HasForeignKey(d => d.changed_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_status_history_changed_by_fkey");

            entity.HasOne(d => d.project).WithMany(p => p.projects_status_history)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_status_history_project_id_fkey");

            entity.HasOne(d => d.status).WithMany(p => p.projects_status_history)
                .HasForeignKey(d => d.status_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_status_history_status_id_fkey");
        });

        modelBuilder.Entity<projects_students>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_students_pkey");

            entity.ToTable("projects_students", "research");

            entity.HasIndex(e => e.project_id, "fk_168644_idx");

            entity.HasIndex(e => e.student_id, "fk_168649_idx");

            entity.HasIndex(e => new { e.project_id, e.student_id }, "projects_students_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.project).WithMany(p => p.projects_students)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_students_project_id_fkey");

            entity.HasOne(d => d.student).WithMany(p => p.projects_students)
                .HasForeignKey(d => d.student_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_students_student_id_fkey");
        });

        modelBuilder.Entity<projects_submissions>(entity =>
        {
            entity.HasKey(e => e.id).HasName("projects_submissions_pkey");

            entity.ToTable("projects_submissions", "reviews");

            entity.HasIndex(e => e.project_id, "fk_169721_idx");

            entity.HasIndex(e => e.committee_id, "fk_169726_idx");

            entity.HasIndex(e => e.submitted_by, "fk_169731_idx");

            entity.HasIndex(e => new { e.application_id, e.project_id }, "fk_169736_idx");

            entity.HasIndex(e => new { e.id, e.committee_id }, "projects_submissions_id_committee_id_key").IsUnique();

            entity.HasIndex(e => e.project_id, "projects_submissions_in_process_uq")
                .IsUnique()
                .HasFilter("(((processing_status)::text = 'in_process'::text) AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.project_id, e.version_number }, "projects_submissions_project_id_version_number_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.processing_status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'in_process'::character varying");
            entity.Property(e => e.snapshot).HasColumnType("jsonb");
            entity.Property(e => e.submitted_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.committee).WithMany(p => p.projects_submissions)
                .HasForeignKey(d => d.committee_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_committee_id_fkey");

            entity.HasOne(d => d.project).WithOne(p => p.projects_submissions)
                .HasForeignKey<projects_submissions>(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_project_id_fkey");

            entity.HasOne(d => d.submitted_byNavigation).WithMany(p => p.projects_submissions)
                .HasForeignKey(d => d.submitted_by)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("projects_submissions_submitted_by_fkey");

            entity.HasOne(d => d.projects_applications).WithMany(p => p.projects_submissions)
                .HasPrincipalKey(p => new { p.id, p.project_id })
                .HasForeignKey(d => new { d.application_id, d.project_id })
                .HasConstraintName("projects_submissions_application_id_project_id_fkey");
        });

        modelBuilder.Entity<research_outputs>(entity =>
        {
            entity.HasKey(e => e.id).HasName("research_outputs_pkey");

            entity.ToTable("research_outputs", "research");

            entity.HasIndex(e => e.project_id, "fk_169436_idx");

            entity.HasIndex(e => e.output_type_id, "fk_169441_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.doi).HasMaxLength(250);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(500);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.output_type).WithMany(p => p.research_outputs)
                .HasForeignKey(d => d.output_type_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_output_type_id_fkey");

            entity.HasOne(d => d.project).WithMany(p => p.research_outputs)
                .HasForeignKey(d => d.project_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_project_id_fkey");
        });

        modelBuilder.Entity<research_outputs_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("research_outputs_documents_pkey");

            entity.ToTable("research_outputs_documents", "research");

            entity.HasIndex(e => e.research_output_id, "fk_169510_idx");

            entity.HasIndex(e => new { e.document_id, e.document_type_id }, "fk_169515_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_current).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.title).HasMaxLength(300);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.version_number).HasDefaultValue(1);

            entity.HasOne(d => d.research_output).WithMany(p => p.research_outputs_documents)
                .HasForeignKey(d => d.research_output_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_documents_research_output_id_fkey");

            entity.HasOne(d => d.documents).WithMany(p => p.research_outputs_documents)
                .HasPrincipalKey(p => new { p.id, p.document_type_id })
                .HasForeignKey(d => new { d.document_id, d.document_type_id })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("research_outputs_documents_document_id_document_type_id_fkey");
        });

        modelBuilder.Entity<research_outputs_researchers>(entity =>
        {
            entity.HasKey(e => e.id).HasName("research_outputs_researchers_pkey");

            entity.ToTable("research_outputs_researchers", "research");

            entity.HasIndex(e => e.research_output_id, "fk_169471_idx");

            entity.HasIndex(e => e.researcher_id, "fk_169476_idx");

            entity.HasIndex(e => new { e.research_output_id, e.author_order }, "research_outputs_researchers_author_order_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => new { e.research_output_id, e.researcher_id }, "research_outputs_researchers_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.research_output).WithMany(p => p.research_outputs_researchers)
                .HasForeignKey(d => d.research_output_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_researchers_research_output_id_fkey");

            entity.HasOne(d => d.researcher).WithMany(p => p.research_outputs_researchers)
                .HasForeignKey(d => d.researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("research_outputs_researchers_researcher_id_fkey");
        });

        modelBuilder.Entity<researchers>(entity =>
        {
            entity.HasKey(e => e.id).HasName("researchers_pkey");

            entity.ToTable("researchers", "academic");

            entity.HasIndex(e => e.user_id, "fk_167872_idx");

            entity.HasIndex(e => e.orcid, "researchers_orcid_key").IsUnique();

            entity.HasIndex(e => e.user_id, "researchers_user_id_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.orcid).HasMaxLength(19);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.user).WithOne(p => p.researchers)
                .HasForeignKey<researchers>(d => d.user_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_user_id_fkey");
        });

        modelBuilder.Entity<researchers_degrees>(entity =>
        {
            entity.HasKey(e => e.id).HasName("researchers_degrees_pkey");

            entity.ToTable("researchers_degrees", "academic");

            entity.HasIndex(e => e.researcher_id, "fk_167934_idx");

            entity.HasIndex(e => e.degree_level_id, "fk_167939_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.degree_title).HasMaxLength(300);
            entity.Property(e => e.institution_name).HasMaxLength(300);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.professional_license).HasMaxLength(100);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.degree_level).WithMany(p => p.researchers_degrees)
                .HasForeignKey(d => d.degree_level_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_degrees_degree_level_id_fkey");

            entity.HasOne(d => d.researcher).WithMany(p => p.researchers_degrees)
                .HasForeignKey(d => d.researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_degrees_researcher_id_fkey");
        });

        modelBuilder.Entity<researchers_experiences>(entity =>
        {
            entity.HasKey(e => e.id).HasName("researchers_experiences_pkey");

            entity.ToTable("researchers_experiences", "academic");

            entity.HasIndex(e => e.researcher_id, "fk_167968_idx");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.organization_name).HasMaxLength(300);
            entity.Property(e => e.position_title).HasMaxLength(250);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.researcher).WithMany(p => p.researchers_experiences)
                .HasForeignKey(d => d.researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_experiences_researcher_id_fkey");
        });

        modelBuilder.Entity<researchers_organizations>(entity =>
        {
            entity.HasKey(e => e.id).HasName("researchers_organizations_pkey");

            entity.ToTable("researchers_organizations", "academic");

            entity.HasIndex(e => e.researcher_id, "fk_167900_idx");

            entity.HasIndex(e => e.organization_id, "fk_167905_idx");

            entity.HasIndex(e => new { e.researcher_id, e.organization_id }, "researchers_organizations_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.employee_number).HasMaxLength(80);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.organization).WithMany(p => p.researchers_organizations)
                .HasForeignKey(d => d.organization_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_organizations_organization_id_fkey");

            entity.HasOne(d => d.researcher).WithMany(p => p.researchers_organizations)
                .HasForeignKey(d => d.researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_organizations_researcher_id_fkey");
        });

        modelBuilder.Entity<researchers_research_lines>(entity =>
        {
            entity.HasKey(e => e.id).HasName("researchers_research_lines_pkey");

            entity.ToTable("researchers_research_lines", "academic");

            entity.HasIndex(e => e.researcher_id, "fk_167994_idx");

            entity.HasIndex(e => e.research_line_id, "fk_167999_idx");

            entity.HasIndex(e => new { e.researcher_id, e.research_line_id }, "researchers_research_lines_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.research_line).WithMany(p => p.researchers_research_lines)
                .HasForeignKey(d => d.research_line_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_research_lines_research_line_id_fkey");

            entity.HasOne(d => d.researcher).WithMany(p => p.researchers_research_lines)
                .HasForeignKey(d => d.researcher_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("researchers_research_lines_researcher_id_fkey");
        });

        modelBuilder.Entity<roles_permissions>(entity =>
        {
            entity.HasKey(e => e.id).HasName("roles_permissions_pkey");

            entity.ToTable("roles_permissions", "access");

            entity.HasIndex(e => e.role_id, "fk_167669_idx");

            entity.HasIndex(e => e.permission_id, "fk_167674_idx");

            entity.HasIndex(e => new { e.role_id, e.permission_id }, "roles_permissions_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.permission).WithMany(p => p.roles_permissions)
                .HasForeignKey(d => d.permission_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("roles_permissions_permission_id_fkey");

            entity.HasOne(d => d.role).WithMany(p => p.roles_permissions)
                .HasForeignKey(d => d.role_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("roles_permissions_role_id_fkey");
        });

        modelBuilder.Entity<students>(entity =>
        {
            entity.HasKey(e => e.id).HasName("students_pkey");

            entity.ToTable("students", "academic");

            entity.HasIndex(e => e.user_id, "fk_168029_idx");

            entity.HasIndex(e => e.academic_program_id, "fk_168034_idx");

            entity.HasIndex(e => new { e.academic_program_id, e.student_number }, "students_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.HasIndex(e => e.user_id, "students_user_id_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.student_number).HasMaxLength(80);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.academic_program).WithMany(p => p.students)
                .HasForeignKey(d => d.academic_program_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("students_academic_program_id_fkey");

            entity.HasOne(d => d.user).WithOne(p => p.students)
                .HasForeignKey<students>(d => d.user_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("students_user_id_fkey");
        });

        modelBuilder.Entity<submissions_documents>(entity =>
        {
            entity.HasKey(e => e.id).HasName("submissions_documents_pkey");

            entity.ToTable("submissions_documents", "reviews");

            entity.HasIndex(e => e.submission_id, "fk_169764_idx");

            entity.HasIndex(e => e.project_document_id, "fk_169769_idx");

            entity.HasIndex(e => new { e.submission_id, e.project_document_id }, "submissions_documents_submission_id_project_document_id_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.project_document).WithMany(p => p.submissions_documents)
                .HasForeignKey(d => d.project_document_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("submissions_documents_project_document_id_fkey");

            entity.HasOne(d => d.submission).WithMany(p => p.submissions_documents)
                .HasForeignKey(d => d.submission_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("submissions_documents_submission_id_fkey");
        });

        modelBuilder.Entity<users>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pkey");

            entity.ToTable("users", "access");

            entity.HasIndex(e => new { e.identity_provider, e.external_subject }, "users_identity_provider_external_subject_key").IsUnique();

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.email).HasMaxLength(320);
            entity.Property(e => e.external_subject).HasMaxLength(250);
            entity.Property(e => e.family_name).HasMaxLength(200);
            entity.Property(e => e.given_name).HasMaxLength(150);
            entity.Property(e => e.identity_provider).HasMaxLength(100);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<users_roles>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_roles_pkey");

            entity.ToTable("users_roles", "access");

            entity.HasIndex(e => e.user_id, "fk_167638_idx");

            entity.HasIndex(e => e.role_id, "fk_167643_idx");

            entity.HasIndex(e => new { e.user_id, e.role_id }, "users_roles_live_uq")
                .IsUnique()
                .HasFilter("(is_active AND (deleted_at IS NULL))");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.is_visible).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.role).WithMany(p => p.users_roles)
                .HasForeignKey(d => d.role_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_roles_role_id_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.users_roles)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_roles_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
