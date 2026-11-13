using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class CallsDocumentRequirements
{
    public long Id { get; set; }

    public long CallId { get; set; }

    public long DocumentTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string WorkflowStage { get; set; } = null!;

    public bool IsRequired { get; set; }

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

    public virtual CCalls Call { get; set; } = null!;

    public virtual CDocumentTypes DocumentType { get; set; } = null!;
}
