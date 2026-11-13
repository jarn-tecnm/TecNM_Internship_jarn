using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class milestones_status_history
{
    public long id { get; set; }

    public long milestone_id { get; set; }

    public long status_id { get; set; }

    public decimal completion_percentage { get; set; }

    public long changed_by { get; set; }

    public DateTime changed_at { get; set; }

    public string reason { get; set; } = null!;

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

    public virtual users changed_byNavigation { get; set; } = null!;

    public virtual milestones milestone { get; set; } = null!;

    public virtual ICollection<milestones_status_history_documents> milestones_status_history_documents { get; set; } = new List<milestones_status_history_documents>();

    public virtual c_milestone_statuses status { get; set; } = null!;
}
