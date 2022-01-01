using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Project")]
    [Index(nameof(Priority))]
    [Index(nameof(StartDate))]
    [Index(nameof(CompletionDate))] 
    public class Project
    {
        [Column("ProjectId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Project name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public ProjectStatus Status { get; set; }
        public int Priority { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
