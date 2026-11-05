using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ResearchersDegree
{
    public long Id { get; set; }

    public long ResearcherId { get; set; }

    public long DegreeLevelId { get; set; }

    public string DegreeTitle { get; set; } = null!;

    public string InstitutionName { get; set; } = null!;

    public int? GraduationYear { get; set; }

    public string? ProfessionalLicense { get; set; }

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

    public virtual CDegreeLevel DegreeLevel { get; set; } = null!;

    public virtual Researcher Researcher { get; set; } = null!;
}
