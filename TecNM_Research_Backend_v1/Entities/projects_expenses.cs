using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_expenses
{
    public long id { get; set; }

    public long budget_item_id { get; set; }

    public long? funding_id { get; set; }

    public string description { get; set; } = null!;

    public DateOnly spent_on { get; set; }

    public decimal amount { get; set; }

    public string? supplier_name { get; set; }

    public string? receipt_number { get; set; }

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

    public virtual projects_budget_items budget_item { get; set; } = null!;

    public virtual projects_funding? funding { get; set; }

    public virtual ICollection<projects_expenses_documents> projects_expenses_documents { get; set; } = new List<projects_expenses_documents>();
}
