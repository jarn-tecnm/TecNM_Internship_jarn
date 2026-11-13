using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class AuditLog
{
    public long Id { get; set; }

    public long? ActorUserId { get; set; }

    public string Action { get; set; } = null!;

    public string EntitySchema { get; set; } = null!;

    public string EntityTable { get; set; } = null!;

    public long? EntityId { get; set; }

    public long? ProjectId { get; set; }

    public DateTime OccurredAt { get; set; }

    public string? RequestId { get; set; }

    public string Details { get; set; } = null!;

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

    public virtual User? ActorUser { get; set; }

    public virtual Project? Project { get; set; }
}
