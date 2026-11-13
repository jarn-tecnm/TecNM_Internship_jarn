using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsApplications
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public long CallId { get; set; }

    public string? ApplicationNumber { get; set; }

    public DateTime? AppliedAt { get; set; }

    public decimal RequestedAmount { get; set; }

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

    public virtual CCalls Call { get; set; } = null!;

    public virtual Projects Project { get; set; } = null!;

    public virtual ICollection<ProjectsApplicationsDocuments> ProjectsApplicationsDocuments { get; set; } = new List<ProjectsApplicationsDocuments>();

    public virtual ICollection<ProjectsFunding> ProjectsFunding { get; set; } = new List<ProjectsFunding>();

    public virtual ICollection<ProjectsSubmissions> ProjectsSubmissions { get; set; } = new List<ProjectsSubmissions>();
}
