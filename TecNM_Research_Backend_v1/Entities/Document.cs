using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Document
{
    public long Id { get; set; }

    public long DocumentTypeId { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string StorageKey { get; set; } = null!;

    public string MediaType { get; set; } = null!;

    public long SizeBytes { get; set; }

    public string Sha256 { get; set; } = null!;

    public long UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsVisible { get; set; }

    public int DisplayOrder { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<AcademicAuthorizationsHistory> AcademicAuthorizationsHistories { get; set; } = new List<AcademicAuthorizationsHistory>();

    public virtual CDocumentType DocumentType { get; set; } = null!;

    public virtual ICollection<ExternalResearchOutputsDocument> ExternalResearchOutputsDocuments { get; set; } = new List<ExternalResearchOutputsDocument>();

    public virtual ICollection<MilestonesStatusHistoryDocument> MilestonesStatusHistoryDocuments { get; set; } = new List<MilestonesStatusHistoryDocument>();

    public virtual ICollection<ObjectivesStatusHistoryDocument> ObjectivesStatusHistoryDocuments { get; set; } = new List<ObjectivesStatusHistoryDocument>();

    public virtual ICollection<ProjectsApplicationsDocument> ProjectsApplicationsDocuments { get; set; } = new List<ProjectsApplicationsDocument>();

    public virtual ICollection<ProjectsBudgetDocument> ProjectsBudgetDocuments { get; set; } = new List<ProjectsBudgetDocument>();

    public virtual ICollection<ProjectsDocument> ProjectsDocuments { get; set; } = new List<ProjectsDocument>();

    public virtual ICollection<ProjectsExpensesDocument> ProjectsExpensesDocuments { get; set; } = new List<ProjectsExpensesDocument>();

    public virtual ICollection<ProjectsFundingDocument> ProjectsFundingDocuments { get; set; } = new List<ProjectsFundingDocument>();

    public virtual ICollection<ProjectsSubmission> ProjectsSubmissions { get; set; } = new List<ProjectsSubmission>();

    public virtual ICollection<ResearchOutputsDocument> ResearchOutputsDocuments { get; set; } = new List<ResearchOutputsDocument>();

    public virtual ICollection<TasksStatusHistoryDocument> TasksStatusHistoryDocuments { get; set; } = new List<TasksStatusHistoryDocument>();

    public virtual User UploadedByNavigation { get; set; } = null!;
}
