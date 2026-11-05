using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class CResearchLine
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long AcademicFieldId { get; set; }

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

    public virtual CAcademicField AcademicField { get; set; } = null!;

    public virtual ICollection<ProjectsResearchLine> ProjectsResearchLines { get; set; } = new List<ProjectsResearchLine>();

    public virtual ICollection<ResearchersResearchLine> ResearchersResearchLines { get; set; } = new List<ResearchersResearchLine>();
}
