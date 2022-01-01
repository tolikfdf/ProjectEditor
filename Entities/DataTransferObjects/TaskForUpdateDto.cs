using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class TaskForUpdateDto
    {
        public string TaskName { get; set; }

        public string TaskDescription { get; set; }
        public Models.TaskStatus Status { get; set; }
        public int Priority { get; set; }
    }
}
