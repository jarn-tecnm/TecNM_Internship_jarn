using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Student
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long AcademicProgramId { get; set; }

    public string StudentNumber { get; set; } = null!;

    public int? Semester { get; set; }

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

    public virtual CAcademicProgram AcademicProgram { get; set; } = null!;

    public virtual ICollection<ProjectsStudent> ProjectsStudents { get; set; } = new List<ProjectsStudent>();

    public virtual User User { get; set; } = null!;
}
