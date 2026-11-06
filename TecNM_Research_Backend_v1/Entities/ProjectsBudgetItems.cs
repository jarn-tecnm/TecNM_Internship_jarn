using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsBudgetItems
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public long BudgetCategoryId { get; set; }

    public string Description { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitCost { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool IsActive { get; set; }

    public bool IsVisible { get; set; }

    public int DisplayOrder { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Notes { get; set; }

    public virtual CBudgetCategories BudgetCategory { get; set; } = null!;

    public virtual Projects Project { get; set; } = null!;

    public virtual ICollection<ProjectsBudgetDocuments> ProjectsBudgetDocuments { get; set; } = new List<ProjectsBudgetDocuments>();

    public virtual ICollection<ProjectsExpenses> ProjectsExpenses { get; set; } = new List<ProjectsExpenses>();
}
