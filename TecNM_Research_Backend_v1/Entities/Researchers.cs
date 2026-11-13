using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class researchers
{
    public long id { get; set; }

    public long user_id { get; set; }

    public string? orcid { get; set; }

    public string? professional_summary { get; set; }

    public string? research_interests { get; set; }

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

    public virtual ICollection<academic_groups_researchers> academic_groups_researchers { get; set; } = new List<academic_groups_researchers>();

    public virtual ICollection<projects_researchers> projects_researchers { get; set; } = new List<projects_researchers>();

    public virtual ICollection<research_outputs_researchers> research_outputs_researchers { get; set; } = new List<research_outputs_researchers>();

    public virtual ICollection<researchers_degrees> researchers_degrees { get; set; } = new List<researchers_degrees>();

    public virtual ICollection<researchers_experiences> researchers_experiences { get; set; } = new List<researchers_experiences>();

    public virtual ICollection<researchers_organizations> researchers_organizations { get; set; } = new List<researchers_organizations>();

    public virtual ICollection<researchers_research_lines> researchers_research_lines { get; set; } = new List<researchers_research_lines>();

    public virtual users user { get; set; } = null!;
}
