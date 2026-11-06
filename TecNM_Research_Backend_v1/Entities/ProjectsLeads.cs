using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsLeads
{
    public long Id { get; set; }

    public long ProjectResearcherId { get; set; }

    public DateTime StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public bool IsCurrent { get; set; }

    public string ChangeReason { get; set; } = null!;

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

    public virtual ProjectsResearchers ProjectResearcher { get; set; } = null!;
}
