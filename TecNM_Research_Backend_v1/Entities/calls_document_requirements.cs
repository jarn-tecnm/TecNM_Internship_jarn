using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class calls_document_requirements
{
    public long id { get; set; }

    public long call_id { get; set; }

    public long document_type_id { get; set; }

    public string title { get; set; } = null!;

    public string workflow_stage { get; set; } = null!;

    public bool is_required { get; set; }

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

    public virtual c_calls call { get; set; } = null!;

    public virtual c_document_types document_type { get; set; } = null!;
}
