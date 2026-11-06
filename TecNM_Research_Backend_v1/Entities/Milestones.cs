using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Milestones
{
    public long Id { get; set; }

    public long ObjectiveId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly DueOn { get; set; }

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

    public virtual ICollection<MilestonesStatusHistory> MilestonesStatusHistory { get; set; } = new List<MilestonesStatusHistory>();

    public virtual Objectives Objective { get; set; } = null!;

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
