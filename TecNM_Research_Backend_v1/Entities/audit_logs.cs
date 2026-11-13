using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class audit_logs
{
    public long id { get; set; }

    public long? actor_user_id { get; set; }

    public string action { get; set; } = null!;

    public string entity_schema { get; set; } = null!;

    public string entity_table { get; set; } = null!;

    public long? entity_id { get; set; }

    public long? project_id { get; set; }

    public DateTime occurred_at { get; set; }

    public string? request_id { get; set; }

    public string details { get; set; } = null!;

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

    public virtual users? actor_user { get; set; }

    public virtual projects? project { get; set; }
}
