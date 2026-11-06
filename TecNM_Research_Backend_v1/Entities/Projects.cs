using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Projects
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

    public virtual ICollection<AuditLogs> AuditLogs { get; set; } = new List<AuditLogs>();

    public virtual CCurrencies Currency { get; set; } = null!;

    public virtual Objectives? Objectives { get; set; }

    public virtual ICollection<ProjectsApplications> ProjectsApplications { get; set; } = new List<ProjectsApplications>();

    public virtual ICollection<ProjectsBudgetItems> ProjectsBudgetItems { get; set; } = new List<ProjectsBudgetItems>();

    public virtual ICollection<ProjectsDocuments> ProjectsDocuments { get; set; } = new List<ProjectsDocuments>();

    public virtual ICollection<ProjectsFunding> ProjectsFunding { get; set; } = new List<ProjectsFunding>();

    public virtual ICollection<ProjectsResearchLines> ProjectsResearchLines { get; set; } = new List<ProjectsResearchLines>();

    public virtual ICollection<ProjectsResearchers> ProjectsResearchers { get; set; } = new List<ProjectsResearchers>();

    public virtual ICollection<ProjectsStatusHistory> ProjectsStatusHistory { get; set; } = new List<ProjectsStatusHistory>();

    public virtual ICollection<ProjectsStudents> ProjectsStudents { get; set; } = new List<ProjectsStudents>();

    public virtual ProjectsSubmissions? ProjectsSubmissions { get; set; }

    public virtual ICollection<ResearchOutputs> ResearchOutputs { get; set; } = new List<ResearchOutputs>();
}
