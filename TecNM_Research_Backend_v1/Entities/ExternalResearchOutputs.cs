using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ExternalResearchOutputs
{
    public long Id { get; set; }

    public long OutputTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? ProducedOn { get; set; }

    public string? Doi { get; set; }

    public string? ExternalUrl { get; set; }

    public string? BibliographicCitation { get; set; }

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

    public virtual ICollection<ExternalResearchOutputsDocuments> ExternalResearchOutputsDocuments { get; set; } = new List<ExternalResearchOutputsDocuments>();

    public virtual ICollection<ExternalResearchOutputsResearchers> ExternalResearchOutputsResearchers { get; set; } = new List<ExternalResearchOutputsResearchers>();

    public virtual CResearchOutputsTypes OutputType { get; set; } = null!;
}
