using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class ProjectsStudents
{
    public long Id { get; set; }

    public long ProjectId { get; set; }

    public long StudentId { get; set; }

    public string? Activities { get; set; }

    public DateOnly StartsOn { get; set; }

    public DateOnly? EndsOn { get; set; }

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

    public virtual Projects Project { get; set; } = null!;

    public virtual Students Student { get; set; } = null!;
}
