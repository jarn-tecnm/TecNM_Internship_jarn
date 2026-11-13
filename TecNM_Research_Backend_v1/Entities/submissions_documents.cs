using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class submissions_documents
{
    public long id { get; set; }

    public long submission_id { get; set; }

    public long project_document_id { get; set; }

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

    public virtual projects_documents project_document { get; set; } = null!;

    public virtual projects_submissions submission { get; set; } = null!;
}
