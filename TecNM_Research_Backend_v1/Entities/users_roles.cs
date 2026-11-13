using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class users_roles
{
    public long id { get; set; }

    public long user_id { get; set; }

    public long role_id { get; set; }

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

    public virtual c_roles role { get; set; } = null!;

    public virtual users user { get; set; } = null!;
}
