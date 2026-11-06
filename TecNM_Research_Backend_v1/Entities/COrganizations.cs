using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class COrganizations
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long OrganizationTypeId { get; set; }

    public string CountryCode { get; set; } = null!;

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

    public virtual ICollection<CAcademicPrograms> CAcademicPrograms { get; set; } = new List<CAcademicPrograms>();

    public virtual ICollection<CCalls> CCalls { get; set; } = new List<CCalls>();

    public virtual COrganizationTypes OrganizationType { get; set; } = null!;

    public virtual ICollection<ResearchersOrganizations> ResearchersOrganizations { get; set; } = new List<ResearchersOrganizations>();
}
