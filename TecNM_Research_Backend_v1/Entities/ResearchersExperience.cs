using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ResearchersExperience
{
    public long Id { get; set; }

    public long ResearcherId { get; set; }

    public string OrganizationName { get; set; } = null!;

    public string PositionTitle { get; set; } = null!;

    public string? Description { get; set; }

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

    public virtual Researcher Researcher { get; set; } = null!;
}
