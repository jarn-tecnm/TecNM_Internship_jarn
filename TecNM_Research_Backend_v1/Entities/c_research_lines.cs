using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class c_research_lines
{
    public long id { get; set; }

    public string code { get; set; } = null!;

    public string name { get; set; } = null!;

    public string? description { get; set; }

    public long academic_field_id { get; set; }

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

    public virtual c_academic_fields academic_field { get; set; } = null!;

    public virtual ICollection<projects_research_lines> projects_research_lines { get; set; } = new List<projects_research_lines>();

    public virtual ICollection<researchers_research_lines> researchers_research_lines { get; set; } = new List<researchers_research_lines>();
}
