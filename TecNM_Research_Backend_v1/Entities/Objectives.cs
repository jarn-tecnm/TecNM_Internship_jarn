using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class objectives
{
    public long id { get; set; }

    public long project_id { get; set; }

    public string objective_type { get; set; } = null!;

    public string description { get; set; } = null!;

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

    public virtual ICollection<milestones> milestones { get; set; } = new List<milestones>();

    public virtual ICollection<objectives_status_history> objectives_status_history { get; set; } = new List<objectives_status_history>();

    public virtual projects project { get; set; } = null!;
}
