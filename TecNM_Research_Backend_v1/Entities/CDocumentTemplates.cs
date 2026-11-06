using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class CDocumentTemplates
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public string StorageKey { get; set; } = null!;

    public string MediaType { get; set; } = null!;

    public long SizeBytes { get; set; }

    public string Sha256 { get; set; } = null!;

    public int VersionNumber { get; set; }

    public bool IsCurrent { get; set; }

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

    public virtual ICollection<CDocumentTypes> CDocumentTypes { get; set; } = new List<CDocumentTypes>();
}
