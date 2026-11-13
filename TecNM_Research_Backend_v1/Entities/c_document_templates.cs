using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class c_document_templates
{
    public long id { get; set; }

    public string code { get; set; } = null!;

    public string name { get; set; } = null!;

    public string original_file_name { get; set; } = null!;

    public string storage_key { get; set; } = null!;

    public string media_type { get; set; } = null!;

    public long size_bytes { get; set; }

    public string sha256 { get; set; } = null!;

    public int version_number { get; set; }

    public bool is_current { get; set; }

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

    public virtual ICollection<c_document_types> c_document_types { get; set; } = new List<c_document_types>();
}
