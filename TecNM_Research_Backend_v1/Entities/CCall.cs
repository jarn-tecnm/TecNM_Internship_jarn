using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class CCall
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long OrganizationId { get; set; }

    public long CallTypeId { get; set; }

    public DateTime OpensAt { get; set; }

    public DateTime ClosesAt { get; set; }

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

    public virtual CCallType CallType { get; set; } = null!;

    public virtual ICollection<CallsDocumentRequirement> CallsDocumentRequirements { get; set; } = new List<CallsDocumentRequirement>();

    public virtual ICollection<CallsRequirement> CallsRequirements { get; set; } = new List<CallsRequirement>();

    public virtual COrganization Organization { get; set; } = null!;

    public virtual ICollection<ProjectsApplication> ProjectsApplications { get; set; } = new List<ProjectsApplication>();
}
