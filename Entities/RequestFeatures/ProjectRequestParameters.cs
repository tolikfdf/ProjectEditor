using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class ProjectRequestParameters : RequestParametersBase
    {
        public ProjectRequestParameters()
        {
            OrderBy = "Name";
        }

        public DateTime MinStartDate { get; set; } = DateTime.MinValue;
        public DateTime MaxStartDate { get; set; } = DateTime.MaxValue;
        public DateTime? MinCompletionDate { get; set; }
        public DateTime? MaxCompletionDate { get; set; }
    }
}
