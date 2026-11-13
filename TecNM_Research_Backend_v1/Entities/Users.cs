using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class users
{
    public long id { get; set; }

    public string identity_provider { get; set; } = null!;

    public string external_subject { get; set; } = null!;

    public string email { get; set; } = null!;

    public string given_name { get; set; } = null!;

    public string family_name { get; set; } = null!;

    public bool is_active { get; set; }

    public bool is_visible { get; set; }

    public int display_order { get; set; }

    public long? created_by { get; set; }

    public long? updated_by { get; set; }

    public long? deleted_by { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    public DateTime? deleted_at { get; set; }

    public string? notes { get; set; }

    public virtual ICollection<academic_authorizations_history> academic_authorizations_history { get; set; } = new List<academic_authorizations_history>();

    public virtual ICollection<academic_groups_status_history> academic_groups_status_history { get; set; } = new List<academic_groups_status_history>();

    public virtual ICollection<activities_status_history> activities_status_history { get; set; } = new List<activities_status_history>();

    public virtual ICollection<audit_logs> audit_logs { get; set; } = new List<audit_logs>();

    public virtual ICollection<committees_members> committees_members { get; set; } = new List<committees_members>();

    public virtual ICollection<documents> documents { get; set; } = new List<documents>();

    public virtual ICollection<milestones_status_history> milestones_status_history { get; set; } = new List<milestones_status_history>();

    public virtual ICollection<objectives_status_history> objectives_status_history { get; set; } = new List<objectives_status_history>();

    public virtual ICollection<projects_status_history> projects_status_history { get; set; } = new List<projects_status_history>();

    public virtual ICollection<projects_submissions> projects_submissions { get; set; } = new List<projects_submissions>();

    public virtual researchers? researchers { get; set; }

    public virtual students? students { get; set; }

    public virtual ICollection<users_roles> users_roles { get; set; } = new List<users_roles>();
}
