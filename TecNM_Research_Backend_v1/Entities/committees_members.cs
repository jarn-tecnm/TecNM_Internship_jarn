using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class committees_members
{
    public long id { get; set; }

    public long committee_id { get; set; }

    public long user_id { get; set; }

    public long committee_role_id { get; set; }

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

    public virtual committees committee { get; set; } = null!;

    public virtual c_committee_roles committee_role { get; set; } = null!;

    public virtual ICollection<projects_reviews> projects_reviews { get; set; } = new List<projects_reviews>();

    public virtual users user { get; set; } = null!;
}
