using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsFundingDocument
{
    public long Id { get; set; }

    public long FundingId { get; set; }

    public long DocumentId { get; set; }

    public long DocumentTypeId { get; set; }

    public string Title { get; set; } = null!;

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

    public virtual Document Document { get; set; } = null!;

    public virtual ProjectsFunding Funding { get; set; } = null!;
}
