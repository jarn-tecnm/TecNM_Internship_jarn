using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_status_history
{
    public long id { get; set; }

    public long project_id { get; set; }

    public long status_id { get; set; }

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

    public virtual projects project { get; set; } = null!;

    public virtual c_project_statuses status { get; set; } = null!;
}
