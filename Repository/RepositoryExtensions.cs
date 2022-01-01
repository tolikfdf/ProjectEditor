using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using Entities.Models;

namespace Repository
{
    public static class RepositoryExtensions
    {
        public static IQueryable<Entities.Models.Task> Sort(this IQueryable<Entities.Models.Task> tasks, string
orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return tasks.OrderBy(e => e.TaskName);
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Entities.Models.Task).GetProperties(BindingFlags.Public |
            BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
                return tasks.OrderBy(e => e.TaskName);
            return tasks.OrderBy(orderQuery);
        }

        public static IQueryable<Project> Sort(this IQueryable<Project> projects, string
orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return projects.OrderBy(e => e.Name);
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Project).GetProperties(BindingFlags.Public |
            BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
                return projects.OrderBy(e => e.Name);
            return projects.OrderBy(orderQuery);
        }

        public static IQueryable<Entities.Models.Task> Filter(this IQueryable<Entities.Models.Task> tasks,
            uint minPriotiry,
            uint maxPriority)
        {
            return tasks.Where(t => (t.Priority >= minPriotiry && t.Priority <= maxPriority));
        }

        public static IQueryable<Project> Filter(this IQueryable<Project> projects,
            uint minPriotiry,
            uint maxPriority,
            DateTime MinStartDate,
            DateTime MaxStartDate,
            DateTime? MinCompletionDate,
            DateTime? MaxCompletionDate
            )
        {
            return projects.Where(t => 
                (t.Priority >= minPriotiry && t.Priority <= maxPriority) &&
                (t.StartDate >= MinStartDate && t.StartDate <= MaxStartDate) &&
                ((MinCompletionDate.HasValue && MaxCompletionDate.HasValue &&
                t.CompletionDate >= MinCompletionDate && t.CompletionDate <= MaxCompletionDate) ||
                (MinCompletionDate.HasValue && !MaxCompletionDate.HasValue &&
                t.CompletionDate >= MinCompletionDate) ||
                (!MinCompletionDate.HasValue && MaxCompletionDate.HasValue &&
                t.CompletionDate <= MaxCompletionDate) ||
                (!MinCompletionDate.HasValue && !MaxCompletionDate.HasValue)
                ));
        }
    }
}
