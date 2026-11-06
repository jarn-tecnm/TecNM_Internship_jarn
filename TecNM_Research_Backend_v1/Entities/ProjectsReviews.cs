using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsReviews
{
    public long Id { get; set; }

    public long SubmissionId { get; set; }

    public long CommitteeId { get; set; }

    public long CommitteeMemberId { get; set; }

    public long StatusId { get; set; }

    public long? OutcomeId { get; set; }

    public string? Assessment { get; set; }

    public DateTime? ReviewedAt { get; set; }

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

    public virtual CommitteesMembers CommitteesMembers { get; set; } = null!;

    public virtual CReviewOutcomes? Outcome { get; set; }

    public virtual ProjectsSubmissions ProjectsSubmissions { get; set; } = null!;

    public virtual CReviewStatuses Status { get; set; } = null!;
}
