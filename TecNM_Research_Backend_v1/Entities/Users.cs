using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Users
{
    public long Id { get; set; }

    public string IdentityProvider { get; set; } = null!;

    public string ExternalSubject { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string GivenName { get; set; } = null!;

    public string FamilyName { get; set; } = null!;

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

    public virtual ICollection<AcademicAuthorizationsHistory> AcademicAuthorizationsHistory { get; set; } = new List<AcademicAuthorizationsHistory>();

    public virtual ICollection<AcademicGroupsStatusHistory> AcademicGroupsStatusHistory { get; set; } = new List<AcademicGroupsStatusHistory>();

    public virtual ICollection<ActivitiesStatusHistory> ActivitiesStatusHistory { get; set; } = new List<ActivitiesStatusHistory>();

    public virtual ICollection<AuditLogs> AuditLogs { get; set; } = new List<AuditLogs>();

    public virtual ICollection<CommitteesMembers> CommitteesMembers { get; set; } = new List<CommitteesMembers>();

    public virtual ICollection<Documents> Documents { get; set; } = new List<Documents>();

    public virtual ICollection<MilestonesStatusHistory> MilestonesStatusHistory { get; set; } = new List<MilestonesStatusHistory>();

    public virtual ICollection<ObjectivesStatusHistory> ObjectivesStatusHistory { get; set; } = new List<ObjectivesStatusHistory>();

    public virtual ICollection<ProjectsStatusHistory> ProjectsStatusHistory { get; set; } = new List<ProjectsStatusHistory>();

    public virtual ICollection<ProjectsSubmissions> ProjectsSubmissions { get; set; } = new List<ProjectsSubmissions>();

    public virtual Researchers? Researchers { get; set; }

    public virtual Students? Students { get; set; }

    public virtual ICollection<UsersRoles> UsersRoles { get; set; } = new List<UsersRoles>();
}
