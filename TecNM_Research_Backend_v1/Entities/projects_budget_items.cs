using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_budget_items
{
    public long id { get; set; }

    public long project_id { get; set; }

    public long budget_category_id { get; set; }

    public string description { get; set; } = null!;

    public decimal quantity { get; set; }

    public decimal unit_cost { get; set; }

    public decimal? total_amount { get; set; }

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

    public virtual c_budget_categories budget_category { get; set; } = null!;

    public virtual projects project { get; set; } = null!;

    public virtual ICollection<projects_budget_documents> projects_budget_documents { get; set; } = new List<projects_budget_documents>();

    public virtual ICollection<projects_expenses> projects_expenses { get; set; } = new List<projects_expenses>();
}
