using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class research_outputs
{
    public long id { get; set; }

    public long project_id { get; set; }

    public long output_type_id { get; set; }

    public string title { get; set; } = null!;

    public string? description { get; set; }

    public DateOnly? produced_on { get; set; }

    public string? doi { get; set; }

    public string? external_url { get; set; }

    public string? bibliographic_citation { get; set; }

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

    public virtual c_research_outputs_types output_type { get; set; } = null!;

    public virtual projects project { get; set; } = null!;

    public virtual ICollection<research_outputs_documents> research_outputs_documents { get; set; } = new List<research_outputs_documents>();

    public virtual ICollection<research_outputs_researchers> research_outputs_researchers { get; set; } = new List<research_outputs_researchers>();
}
