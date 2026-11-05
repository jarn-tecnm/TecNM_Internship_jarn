using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Objective
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public string ObjectiveType { get; set; } = null!;

    public string Description { get; set; } = null!;

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

    public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();

    public virtual ICollection<ObjectivesStatusHistory> ObjectivesStatusHistories { get; set; } = new List<ObjectivesStatusHistory>();

    public virtual Project Project { get; set; } = null!;
}
