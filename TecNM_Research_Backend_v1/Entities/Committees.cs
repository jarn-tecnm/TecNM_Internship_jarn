using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class committees
{
    public long id { get; set; }

    public string name { get; set; } = null!;

    public long? academic_program_id { get; set; }

    public DateOnly starts_on { get; set; }

    public DateOnly? ends_on { get; set; }

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

    public virtual c_academic_programs? academic_program { get; set; }

    public virtual ICollection<committees_members> committees_members { get; set; } = new List<committees_members>();

    public virtual ICollection<projects_submissions> projects_submissions { get; set; } = new List<projects_submissions>();
}
