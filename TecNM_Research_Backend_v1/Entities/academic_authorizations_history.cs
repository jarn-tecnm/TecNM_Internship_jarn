using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class academic_authorizations_history
{
    public long id { get; set; }

    public long submission_id { get; set; }

    public long status_id { get; set; }

    public long authorized_by { get; set; }

    public DateTime decided_at { get; set; }

    public string rationale { get; set; } = null!;

    public long document_id { get; set; }

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

    public virtual users authorized_byNavigation { get; set; } = null!;

    public virtual documents document { get; set; } = null!;

    public virtual c_academic_authorization_statuses status { get; set; } = null!;

    public virtual projects_submissions submission { get; set; } = null!;
}
