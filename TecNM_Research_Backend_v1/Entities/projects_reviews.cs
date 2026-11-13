using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class projects_reviews
{
    public long id { get; set; }

    public long submission_id { get; set; }

    public long committee_id { get; set; }

    public long committee_member_id { get; set; }

    public long status_id { get; set; }

    public long? outcome_id { get; set; }

    public string? assessment { get; set; }

    public DateTime? reviewed_at { get; set; }

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

    public virtual committees_members committees_members { get; set; } = null!;

    public virtual c_review_outcomes? outcome { get; set; }

    public virtual projects_submissions projects_submissions { get; set; } = null!;

    public virtual c_review_statuses status { get; set; } = null!;
}
