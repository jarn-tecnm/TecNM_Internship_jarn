using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ResearchersResearchLine
{
    public long Id { get; set; }

    public long ResearcherId { get; set; }

    public long ResearchLineId { get; set; }

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

    public virtual CResearchLine ResearchLine { get; set; } = null!;

    public virtual Researcher Researcher { get; set; } = null!;
}
