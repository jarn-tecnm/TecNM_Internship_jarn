using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsResearcher
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public long ResearcherId { get; set; }

    public long ResearcherRoleId { get; set; }

    public string? Contribution { get; set; }

    public DateOnly StartsOn { get; set; }

    public DateOnly? EndsOn { get; set; }

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

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectsLead? ProjectsLead { get; set; }

    public virtual Researcher Researcher { get; set; } = null!;

    public virtual CResearchersRole ResearcherRole { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
