using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Project
{
    public long Id { get; set; }

    public string ReferenceCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public long CurrencyId { get; set; }

    public string? Abstract { get; set; }

    public string? ProblemStatement { get; set; }

    public string? Justification { get; set; }

    public string? TheoreticalFramework { get; set; }

    public string? Methodology { get; set; }

    public string? ExpectedResults { get; set; }

    public string? BibliographicReferences { get; set; }

    public DateOnly StartsOn { get; set; }

    public DateOnly EndsOn { get; set; }

    public decimal ObjectivesProgressPercentage { get; set; }

    public decimal MilestonesProgressPercentage { get; set; }

    public decimal TasksProgressPercentage { get; set; }

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

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual CCurrency Currency { get; set; } = null!;

    public virtual Objective? Objective { get; set; }

    public virtual ICollection<ProjectsApplication> ProjectsApplications { get; set; } = new List<ProjectsApplication>();

    public virtual ICollection<ProjectsBudgetItem> ProjectsBudgetItems { get; set; } = new List<ProjectsBudgetItem>();

    public virtual ICollection<ProjectsDocument> ProjectsDocuments { get; set; } = new List<ProjectsDocument>();

    public virtual ICollection<ProjectsFunding> ProjectsFundings { get; set; } = new List<ProjectsFunding>();

    public virtual ICollection<ProjectsResearchLine> ProjectsResearchLines { get; set; } = new List<ProjectsResearchLine>();

    public virtual ICollection<ProjectsResearcher> ProjectsResearchers { get; set; } = new List<ProjectsResearcher>();

    public virtual ICollection<ProjectsStatusHistory> ProjectsStatusHistories { get; set; } = new List<ProjectsStatusHistory>();

    public virtual ICollection<ProjectsStudent> ProjectsStudents { get; set; } = new List<ProjectsStudent>();

    public virtual ProjectsSubmission? ProjectsSubmission { get; set; }

    public virtual ICollection<ResearchOutput> ResearchOutputs { get; set; } = new List<ResearchOutput>();
}
