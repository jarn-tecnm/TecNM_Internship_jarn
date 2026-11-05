using System;
using System.Collections.Generic;

namespace TecNM.Research.Entities;

public partial class Task
{
    public long Id { get; set; }

    public long MilestoneId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly StartsOn { get; set; }

    public DateOnly EndsOn { get; set; }

    public long? ResponsibleParticipantId { get; set; }

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

    public virtual Milestone Milestone { get; set; } = null!;

    public virtual ProjectsResearcher? ResponsibleParticipant { get; set; }

    public virtual ICollection<TasksStatusHistory> TasksStatusHistories { get; set; } = new List<TasksStatusHistory>();
}
