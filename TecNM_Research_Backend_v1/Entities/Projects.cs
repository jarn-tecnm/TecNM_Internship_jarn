using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects
{
    public long id { get; set; }

    public string reference_code { get; set; } = null!;

    public string title { get; set; } = null!;

    public long organization_id { get; set; }

    public bool is_managed_in_platform { get; set; }

    public long currency_id { get; set; }

    public string? _abstract { get; set; }

    public string? problem_statement { get; set; }

    public string? justification { get; set; }

    public string? theoretical_framework { get; set; }

    public string? methodology { get; set; }

    public string? expected_results { get; set; }

    public string? bibliographic_references { get; set; }

    public DateOnly starts_on { get; set; }

    public DateOnly ends_on { get; set; }

    public decimal objectives_progress_percentage { get; set; }

    public decimal milestones_progress_percentage { get; set; }

    public decimal activities_progress_percentage { get; set; }

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

    public virtual ICollection<audit_logs> audit_logs { get; set; } = new List<audit_logs>();

    public virtual c_currencies currency { get; set; } = null!;

    public virtual objectives? objectives { get; set; }

    public virtual c_organizations organization { get; set; } = null!;

    public virtual ICollection<projects_academic_groups> projects_academic_groups { get; set; } = new List<projects_academic_groups>();

    public virtual ICollection<projects_applications> projects_applications { get; set; } = new List<projects_applications>();

    public virtual ICollection<projects_budget_items> projects_budget_items { get; set; } = new List<projects_budget_items>();

    public virtual ICollection<projects_documents> projects_documents { get; set; } = new List<projects_documents>();

    public virtual ICollection<projects_funding> projects_funding { get; set; } = new List<projects_funding>();

    public virtual ICollection<projects_research_lines> projects_research_lines { get; set; } = new List<projects_research_lines>();

    public virtual ICollection<projects_researchers> projects_researchers { get; set; } = new List<projects_researchers>();

    public virtual ICollection<projects_status_history> projects_status_history { get; set; } = new List<projects_status_history>();

    public virtual ICollection<projects_students> projects_students { get; set; } = new List<projects_students>();

    public virtual projects_submissions? projects_submissions { get; set; }

    public virtual ICollection<research_outputs> research_outputs { get; set; } = new List<research_outputs>();
}
