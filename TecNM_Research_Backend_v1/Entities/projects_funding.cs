using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_funding
{
    public long id { get; set; }

    public long project_id { get; set; }

    public long funding_source_id { get; set; }

    public long? application_id { get; set; }

    public string? reference_number { get; set; }

    public decimal requested_amount { get; set; }

    public decimal approved_amount { get; set; }

    public decimal received_amount { get; set; }

    public DateOnly? awarded_on { get; set; }

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

    public virtual c_funding_sources funding_source { get; set; } = null!;

    public virtual projects project { get; set; } = null!;

    public virtual projects_applications? projects_applications { get; set; }

    public virtual ICollection<projects_expenses> projects_expenses { get; set; } = new List<projects_expenses>();

    public virtual ICollection<projects_funding_documents> projects_funding_documents { get; set; } = new List<projects_funding_documents>();
}
