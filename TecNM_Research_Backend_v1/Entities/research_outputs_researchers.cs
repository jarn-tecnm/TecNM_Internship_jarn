using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class research_outputs_researchers
{
    public long id { get; set; }

    public long research_output_id { get; set; }

    public long researcher_id { get; set; }

    public int author_order { get; set; }

    public bool is_corresponding_author { get; set; }

    public string? contribution { get; set; }

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

    public virtual research_outputs research_output { get; set; } = null!;

    public virtual researchers researcher { get; set; } = null!;
}
