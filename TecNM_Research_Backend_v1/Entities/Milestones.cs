using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class milestones
{
    public long id { get; set; }

    public long objective_id { get; set; }

    public string title { get; set; } = null!;

    public string? description { get; set; }

    public DateOnly due_on { get; set; }

    public bool is_active { get; set; }

    public bool is_visible { get; set; }

    public int display_order { get; set; }

    public long? created_by { get; set; }

    public long? updated_by { get; set; }

    public long? deleted_by { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    public DateTime? deleted_at { get; set; }

    public string? notes { get; set; }

    public virtual ICollection<activities> activities { get; set; } = new List<activities>();

    public virtual ICollection<milestones_status_history> milestones_status_history { get; set; } = new List<milestones_status_history>();

    public virtual objectives objective { get; set; } = null!;
}
