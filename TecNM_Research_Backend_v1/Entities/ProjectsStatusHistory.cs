using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsStatusHistory
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public long StatusId { get; set; }

    public long ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; }

    public string Reason { get; set; } = null!;

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

    public virtual User ChangedByNavigation { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual CProjectStatus Status { get; set; } = null!;
}
