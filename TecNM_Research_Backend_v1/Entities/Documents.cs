using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Documents
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

    public virtual ICollection<AcademicAuthorizationsHistory> AcademicAuthorizationsHistory { get; set; } = new List<AcademicAuthorizationsHistory>();

    public virtual CDocumentTypes DocumentType { get; set; } = null!;

    public virtual ICollection<ExternalResearchOutputsDocuments> ExternalResearchOutputsDocuments { get; set; } = new List<ExternalResearchOutputsDocuments>();

    public virtual ICollection<MilestonesStatusHistoryDocuments> MilestonesStatusHistoryDocuments { get; set; } = new List<MilestonesStatusHistoryDocuments>();

    public virtual ICollection<ObjectivesStatusHistoryDocuments> ObjectivesStatusHistoryDocuments { get; set; } = new List<ObjectivesStatusHistoryDocuments>();

    public virtual ICollection<ProjectsApplicationsDocuments> ProjectsApplicationsDocuments { get; set; } = new List<ProjectsApplicationsDocuments>();

    public virtual ICollection<ProjectsBudgetDocuments> ProjectsBudgetDocuments { get; set; } = new List<ProjectsBudgetDocuments>();

    public virtual ICollection<ProjectsDocuments> ProjectsDocuments { get; set; } = new List<ProjectsDocuments>();

    public virtual ICollection<ProjectsExpensesDocuments> ProjectsExpensesDocuments { get; set; } = new List<ProjectsExpensesDocuments>();

    public virtual ICollection<ProjectsFundingDocuments> ProjectsFundingDocuments { get; set; } = new List<ProjectsFundingDocuments>();

    public virtual ICollection<ProjectsSubmissions> ProjectsSubmissions { get; set; } = new List<ProjectsSubmissions>();

    public virtual ICollection<ResearchOutputsDocuments> ResearchOutputsDocuments { get; set; } = new List<ResearchOutputsDocuments>();

    public virtual ICollection<TasksStatusHistoryDocuments> TasksStatusHistoryDocuments { get; set; } = new List<TasksStatusHistoryDocuments>();

    public virtual Users UploadedByNavigation { get; set; } = null!;
}
