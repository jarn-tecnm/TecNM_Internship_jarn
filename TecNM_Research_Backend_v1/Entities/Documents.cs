using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class documents
{
    public long id { get; set; }

    public long document_type_id { get; set; }

    public string original_file_name { get; set; } = null!;

    public string storage_key { get; set; } = null!;

    public string media_type { get; set; } = null!;

    public long size_bytes { get; set; }

    public string sha256 { get; set; } = null!;

    public long uploaded_by { get; set; }

    public DateTime uploaded_at { get; set; }

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

    public virtual ICollection<academic_authorizations_history> academic_authorizations_history { get; set; } = new List<academic_authorizations_history>();

    public virtual ICollection<activities_status_history_documents> activities_status_history_documents { get; set; } = new List<activities_status_history_documents>();

    public virtual c_document_types document_type { get; set; } = null!;

    public virtual ICollection<milestones_status_history_documents> milestones_status_history_documents { get; set; } = new List<milestones_status_history_documents>();

    public virtual ICollection<objectives_status_history_documents> objectives_status_history_documents { get; set; } = new List<objectives_status_history_documents>();

    public virtual ICollection<projects_applications_documents> projects_applications_documents { get; set; } = new List<projects_applications_documents>();

    public virtual ICollection<projects_budget_documents> projects_budget_documents { get; set; } = new List<projects_budget_documents>();

    public virtual ICollection<projects_documents> projects_documents { get; set; } = new List<projects_documents>();

    public virtual ICollection<projects_expenses_documents> projects_expenses_documents { get; set; } = new List<projects_expenses_documents>();

    public virtual ICollection<projects_funding_documents> projects_funding_documents { get; set; } = new List<projects_funding_documents>();

    public virtual ICollection<research_outputs_documents> research_outputs_documents { get; set; } = new List<research_outputs_documents>();

    public virtual users uploaded_byNavigation { get; set; } = null!;
}
