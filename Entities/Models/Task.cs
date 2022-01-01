using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Task")]
    [Index(nameof(Priority))]
    public class Task
    {
        [Column("TaskId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Task name is required.")]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Task description is required.")]
        public string TaskDescription { get; set; }

        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public TaskStatus Status { get; set; }
        public int Priority { get; set; }
    }
}
