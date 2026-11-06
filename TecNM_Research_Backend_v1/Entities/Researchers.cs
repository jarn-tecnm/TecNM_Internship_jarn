using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Researchers
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

    public virtual ICollection<ExternalResearchOutputsResearchers> ExternalResearchOutputsResearchers { get; set; } = new List<ExternalResearchOutputsResearchers>();

    public virtual ICollection<ProjectsResearchers> ProjectsResearchers { get; set; } = new List<ProjectsResearchers>();

    public virtual ICollection<ResearchOutputsResearchers> ResearchOutputsResearchers { get; set; } = new List<ResearchOutputsResearchers>();

    public virtual ICollection<ResearchersDegrees> ResearchersDegrees { get; set; } = new List<ResearchersDegrees>();

    public virtual ICollection<ResearchersExperiences> ResearchersExperiences { get; set; } = new List<ResearchersExperiences>();

    public virtual ICollection<ResearchersOrganizations> ResearchersOrganizations { get; set; } = new List<ResearchersOrganizations>();

    public virtual ICollection<ResearchersResearchLines> ResearchersResearchLines { get; set; } = new List<ResearchersResearchLines>();

    public virtual Users User { get; set; } = null!;
}
