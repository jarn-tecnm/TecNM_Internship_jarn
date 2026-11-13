using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_submissions
{
    public long id { get; set; }

    public long project_id { get; set; }

    public long committee_id { get; set; }

    public long? application_id { get; set; }

    public int version_number { get; set; }

    public long submitted_by { get; set; }

    public DateTime submitted_at { get; set; }

    public string processing_status { get; set; } = null!;

    public string snapshot { get; set; } = null!;

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

    public virtual committees committee { get; set; } = null!;

    public virtual projects project { get; set; } = null!;

    public virtual projects_applications? projects_applications { get; set; }

    public virtual ICollection<projects_reviews> projects_reviews { get; set; } = new List<projects_reviews>();

    public virtual ICollection<submissions_documents> submissions_documents { get; set; } = new List<submissions_documents>();

    public virtual users submitted_byNavigation { get; set; } = null!;
}
