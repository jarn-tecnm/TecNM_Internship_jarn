using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class academic_groups_status_history
{
    public long id { get; set; }

    public long academic_group_id { get; set; }

    public long level_id { get; set; }

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

    public virtual academic_groups academic_group { get; set; } = null!;

    public virtual users changed_byNavigation { get; set; } = null!;

    public virtual c_academic_group_levels level { get; set; } = null!;
}
