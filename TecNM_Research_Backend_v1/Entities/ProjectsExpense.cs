using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsExpense
{
    public long Id { get; set; }

    public long BudgetItemId { get; set; }

    public long? FundingId { get; set; }

    public string Description { get; set; } = null!;

    public DateOnly SpentOn { get; set; }

    public decimal Amount { get; set; }

    public string? SupplierName { get; set; }

    public string? ReceiptNumber { get; set; }

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

    public virtual ProjectsBudgetItem BudgetItem { get; set; } = null!;

    public virtual ProjectsFunding? Funding { get; set; }

    public virtual ICollection<ProjectsExpensesDocument> ProjectsExpensesDocuments { get; set; } = new List<ProjectsExpensesDocument>();
}
