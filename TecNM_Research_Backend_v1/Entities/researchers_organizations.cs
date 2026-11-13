using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class researchers_organizations
{
    public long id { get; set; }

    public long researcher_id { get; set; }

    public long organization_id { get; set; }

    public string? employee_number { get; set; }

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

    public virtual c_organizations organization { get; set; } = null!;

    public virtual researchers researcher { get; set; } = null!;
}
