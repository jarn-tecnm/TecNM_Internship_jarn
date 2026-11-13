using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsFunding
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public long FundingSourceId { get; set; }

    public long? ApplicationId { get; set; }

    public string? ReferenceNumber { get; set; }

    public decimal RequestedAmount { get; set; }

    public decimal ApprovedAmount { get; set; }

    public decimal ReceivedAmount { get; set; }

    public DateOnly? AwardedOn { get; set; }

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

    public virtual CFundingSources FundingSource { get; set; } = null!;

    public virtual Projects Project { get; set; } = null!;

    public virtual ProjectsApplications? ProjectsApplications { get; set; }

    public virtual ICollection<ProjectsExpenses> ProjectsExpenses { get; set; } = new List<ProjectsExpenses>();

    public virtual ICollection<ProjectsFundingDocuments> ProjectsFundingDocuments { get; set; } = new List<ProjectsFundingDocuments>();
}
