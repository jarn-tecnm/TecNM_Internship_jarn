using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class SubmissionsDocument
{
    public long Id { get; set; }

    public long SubmissionId { get; set; }

    public long ProjectDocumentId { get; set; }

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

    public virtual ProjectsDocument ProjectDocument { get; set; } = null!;

    public virtual ProjectsSubmission Submission { get; set; } = null!;
}
