using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class activities_current_status
{
    public long? id { get; set; }

    public long? activity_id { get; set; }

    public long? status_id { get; set; }

    public decimal? completion_percentage { get; set; }

    public long? changed_by { get; set; }

    public DateTime? changed_at { get; set; }

    public string? reason { get; set; }

    public bool? is_active { get; set; }

    public bool? is_visible { get; set; }

    public int? display_order { get; set; }

    public long? created_by { get; set; }

    public long? updated_by { get; set; }

    public long? deleted_by { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public DateTime? deleted_at { get; set; }

    public string? notes { get; set; }
}
