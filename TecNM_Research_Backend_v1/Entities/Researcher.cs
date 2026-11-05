using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Researcher
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string? Orcid { get; set; }

    public string? ProfessionalSummary { get; set; }

    public string? ResearchInterests { get; set; }

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

    public virtual ICollection<ExternalResearchOutputsResearcher> ExternalResearchOutputsResearchers { get; set; } = new List<ExternalResearchOutputsResearcher>();

    public virtual ICollection<ProjectsResearcher> ProjectsResearchers { get; set; } = new List<ProjectsResearcher>();

    public virtual ICollection<ResearchOutputsResearcher> ResearchOutputsResearchers { get; set; } = new List<ResearchOutputsResearcher>();

    public virtual ICollection<ResearchersDegree> ResearchersDegrees { get; set; } = new List<ResearchersDegree>();

    public virtual ICollection<ResearchersExperience> ResearchersExperiences { get; set; } = new List<ResearchersExperience>();

    public virtual ICollection<ResearchersOrganization> ResearchersOrganizations { get; set; } = new List<ResearchersOrganization>();

    public virtual ICollection<ResearchersResearchLine> ResearchersResearchLines { get; set; } = new List<ResearchersResearchLine>();

    public virtual User User { get; set; } = null!;
}
