using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class c_organizations
{
    public long id { get; set; }

    public string code { get; set; } = null!;

    public string name { get; set; } = null!;

    public string? description { get; set; }

    public long organization_type_id { get; set; }

    public string country_code { get; set; } = null!;

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

    public virtual ICollection<academic_groups> academic_groups { get; set; } = new List<academic_groups>();

    public virtual ICollection<c_academic_programs> c_academic_programs { get; set; } = new List<c_academic_programs>();

    public virtual ICollection<c_calls> c_calls { get; set; } = new List<c_calls>();

    public virtual c_organization_types organization_type { get; set; } = null!;

    public virtual ICollection<projects> projects { get; set; } = new List<projects>();

    public virtual ICollection<researchers_organizations> researchers_organizations { get; set; } = new List<researchers_organizations>();
}
