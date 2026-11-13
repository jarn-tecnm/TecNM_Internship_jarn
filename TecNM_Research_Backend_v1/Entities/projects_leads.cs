using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_leads
{
    public long id { get; set; }

    public long project_researcher_id { get; set; }

    public DateTime starts_at { get; set; }

    public DateTime? ends_at { get; set; }

    public bool is_current { get; set; }

    public string change_reason { get; set; } = null!;

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

    public virtual projects_researchers project_researcher { get; set; } = null!;
}
