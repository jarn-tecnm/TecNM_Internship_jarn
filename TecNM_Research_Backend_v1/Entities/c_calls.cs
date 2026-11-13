using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class c_calls
{
    public long id { get; set; }

    public string code { get; set; } = null!;

    public string name { get; set; } = null!;

    public string? description { get; set; }

    public long organization_id { get; set; }

    public long call_type_id { get; set; }

    public DateTime opens_at { get; set; }

    public DateTime closes_at { get; set; }

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

    public virtual c_call_types call_type { get; set; } = null!;

    public virtual ICollection<calls_document_requirements> calls_document_requirements { get; set; } = new List<calls_document_requirements>();

    public virtual ICollection<calls_requirements> calls_requirements { get; set; } = new List<calls_requirements>();

    public virtual c_organizations organization { get; set; } = null!;

    public virtual ICollection<projects_applications> projects_applications { get; set; } = new List<projects_applications>();
}
