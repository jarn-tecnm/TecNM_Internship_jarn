using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class User
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

    public virtual ICollection<AcademicAuthorizationsHistory> AcademicAuthorizationsHistories { get; set; } = new List<AcademicAuthorizationsHistory>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<CommitteesMember> CommitteesMembers { get; set; } = new List<CommitteesMember>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<MilestonesStatusHistory> MilestonesStatusHistories { get; set; } = new List<MilestonesStatusHistory>();

    public virtual ICollection<ObjectivesStatusHistory> ObjectivesStatusHistories { get; set; } = new List<ObjectivesStatusHistory>();

    public virtual ICollection<ProjectsStatusHistory> ProjectsStatusHistories { get; set; } = new List<ProjectsStatusHistory>();

    public virtual ICollection<ProjectsSubmission> ProjectsSubmissionAcademicAuthorizedByNavigations { get; set; } = new List<ProjectsSubmission>();

    public virtual ICollection<ProjectsSubmission> ProjectsSubmissionSubmittedByNavigations { get; set; } = new List<ProjectsSubmission>();

    public virtual Researcher? Researcher { get; set; }

    public virtual Student? Student { get; set; }

    public virtual ICollection<TasksStatusHistory> TasksStatusHistories { get; set; } = new List<TasksStatusHistory>();

    public virtual ICollection<UsersRole> UsersRoles { get; set; } = new List<UsersRole>();
}
