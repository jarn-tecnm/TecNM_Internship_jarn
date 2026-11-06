using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsSubmissions
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public long CommitteeId { get; set; }

    public long? ApplicationId { get; set; }

    public int VersionNumber { get; set; }

    public long SubmittedBy { get; set; }

    public DateTime SubmittedAt { get; set; }

    public string ProcessingStatus { get; set; } = null!;

    public string Snapshot { get; set; } = null!;

    public long? AcademicStatusId { get; set; }

    public long? AcademicAuthorizedBy { get; set; }

    public DateTime? AcademicDecidedAt { get; set; }

    public string? AcademicRationale { get; set; }

    public long? AcademicDocumentId { get; set; }

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

    public virtual Users? AcademicAuthorizedByNavigation { get; set; }

    public virtual Documents? AcademicDocument { get; set; }

    public virtual CAcademicAuthorizationStatuses? AcademicStatus { get; set; }

    public virtual Committees Committee { get; set; } = null!;

    public virtual Projects Project { get; set; } = null!;

    public virtual ProjectsApplications? ProjectsApplications { get; set; }

    public virtual ICollection<ProjectsReviews> ProjectsReviews { get; set; } = new List<ProjectsReviews>();

    public virtual ICollection<SubmissionsDocuments> SubmissionsDocuments { get; set; } = new List<SubmissionsDocuments>();

    public virtual Users SubmittedByNavigation { get; set; } = null!;
}
