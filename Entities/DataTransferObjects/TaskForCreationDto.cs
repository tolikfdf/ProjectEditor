using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class TaskForCreationDto
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public Models.TaskStatus Status { get; set; }

        public Guid ProjectId { get; set; }
        public int Priority { get; set; }
    }
}
