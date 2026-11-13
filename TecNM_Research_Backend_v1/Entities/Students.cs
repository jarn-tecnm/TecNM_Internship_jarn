using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class students
{
    public long id { get; set; }

    public long user_id { get; set; }

    public long academic_program_id { get; set; }

    public string student_number { get; set; } = null!;

    public int? semester { get; set; }

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

    public virtual c_academic_programs academic_program { get; set; } = null!;

    public virtual ICollection<projects_students> projects_students { get; set; } = new List<projects_students>();

    public virtual users user { get; set; } = null!;
}
