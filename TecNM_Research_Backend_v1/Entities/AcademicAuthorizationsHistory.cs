using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class AcademicAuthorizationsHistory
{
    public long Id { get; set; }

    public long SubmissionId { get; set; }

    public long StatusId { get; set; }

    public long AuthorizedBy { get; set; }

    public DateTime DecidedAt { get; set; }

    public string Rationale { get; set; } = null!;

    public long DocumentId { get; set; }

    public DateTime ReplacedAt { get; set; }

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

    public virtual User AuthorizedByNavigation { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;

    public virtual CAcademicAuthorizationStatus Status { get; set; } = null!;

    public virtual ProjectsSubmission Submission { get; set; } = null!;
}
