using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_researchers
{
    public long id { get; set; }

    public long project_id { get; set; }

    public long researcher_id { get; set; }

    public long researcher_role_id { get; set; }

    public string? contribution { get; set; }

    public DateOnly starts_on { get; set; }

    public DateOnly? ends_on { get; set; }

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

    public virtual projects project { get; set; } = null!;

    public virtual projects_leads? projects_leads { get; set; }

    public virtual researchers researcher { get; set; } = null!;

    public virtual c_researchers_roles researcher_role { get; set; } = null!;
}
