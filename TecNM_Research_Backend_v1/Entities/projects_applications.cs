using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_applications
{
    public long id { get; set; }

    public long project_id { get; set; }

    public long call_id { get; set; }

    public string? application_number { get; set; }

    public DateTime? applied_at { get; set; }

    public decimal requested_amount { get; set; }

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

    public virtual projects project { get; set; } = null!;

    public virtual ICollection<projects_applications_documents> projects_applications_documents { get; set; } = new List<projects_applications_documents>();

    public virtual ICollection<projects_funding> projects_funding { get; set; } = new List<projects_funding>();

    public virtual ICollection<projects_submissions> projects_submissions { get; set; } = new List<projects_submissions>();
}
