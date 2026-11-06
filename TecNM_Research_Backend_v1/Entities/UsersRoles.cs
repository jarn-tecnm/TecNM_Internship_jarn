using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class UsersRoles
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long RoleId { get; set; }

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

    public virtual CRoles Role { get; set; } = null!;

    public virtual Users User { get; set; } = null!;
}
