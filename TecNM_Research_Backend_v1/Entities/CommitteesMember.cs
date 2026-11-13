using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class CommitteesMember
{
    public long Id { get; set; }

    public long CommitteeId { get; set; }

    public long UserId { get; set; }

    public long CommitteeRoleId { get; set; }

    public DateOnly StartsOn { get; set; }

    public DateOnly? EndsOn { get; set; }

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

    public virtual Committee Committee { get; set; } = null!;

    public virtual CCommitteeRole CommitteeRole { get; set; } = null!;

    public virtual ICollection<ProjectsReview> ProjectsReviews { get; set; } = new List<ProjectsReview>();

    public virtual User User { get; set; } = null!;
}
