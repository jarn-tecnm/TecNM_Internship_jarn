using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class researchers_degrees
{
    public long id { get; set; }

    public long researcher_id { get; set; }

    public long degree_level_id { get; set; }

    public string degree_title { get; set; } = null!;

    public string institution_name { get; set; } = null!;

    public int? graduation_year { get; set; }

    public string? professional_license { get; set; }

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

    public virtual c_degree_levels degree_level { get; set; } = null!;

    public virtual researchers researcher { get; set; } = null!;
}
