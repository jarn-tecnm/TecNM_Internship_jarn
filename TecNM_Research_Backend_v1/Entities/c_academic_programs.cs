using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class c_academic_programs
{
    public long id { get; set; }

    public string code { get; set; } = null!;

    public string name { get; set; } = null!;

    public string? description { get; set; }

    public long organization_id { get; set; }

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

    public virtual ICollection<committees> committees { get; set; } = new List<committees>();

    public virtual c_organizations organization { get; set; } = null!;

    public virtual ICollection<students> students { get; set; } = new List<students>();
}
