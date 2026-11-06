using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Committees
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? AcademicProgramId { get; set; }

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

    public virtual CAcademicPrograms? AcademicProgram { get; set; }

    public virtual ICollection<CommitteesMembers> CommitteesMembers { get; set; } = new List<CommitteesMembers>();

    public virtual ICollection<ProjectsSubmissions> ProjectsSubmissions { get; set; } = new List<ProjectsSubmissions>();
}
