using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class research_outputs_documents
{
    public long id { get; set; }

    public long research_output_id { get; set; }

    public long document_id { get; set; }

    public long document_type_id { get; set; }

    public string title { get; set; } = null!;

    public int version_number { get; set; }

    public bool is_current { get; set; }

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

    public virtual documents documents { get; set; } = null!;

    public virtual research_outputs research_output { get; set; } = null!;
}
