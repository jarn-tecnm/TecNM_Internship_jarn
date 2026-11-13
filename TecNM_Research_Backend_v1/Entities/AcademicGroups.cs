using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class AcademicGroups
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long OrganizationId { get; set; }

    public string? Description { get; set; }

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

    public virtual AcademicGroupsResearchers? AcademicGroupsResearchers { get; set; }

    public virtual ICollection<AcademicGroupsStatusHistory> AcademicGroupsStatusHistory { get; set; } = new List<AcademicGroupsStatusHistory>();

    public virtual COrganizations Organization { get; set; } = null!;

    public virtual ICollection<ProjectsAcademicGroups> ProjectsAcademicGroups { get; set; } = new List<ProjectsAcademicGroups>();
}
