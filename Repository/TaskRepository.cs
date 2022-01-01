using Contracts;
using Entities;
using Entities.RequestFeatures;

namespace Repository
{
    public class TaskRepository : RepositoryBase<Entities.Models.Task>, ITaskRepository
    {
        public TaskRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateTask(Guid projectId, Entities.Models.Task taskEntity)
        {
            taskEntity.ProjectId = projectId;
            Create(taskEntity);
        }

        public void DeleteTask(Entities.Models.Task task) => Delete(task);

        public Entities.Models.Task GetTaskById(Guid projectId, Guid Id, bool trackChanges) =>
            FindByCondition(x => x.ProjectId.Equals(projectId) && x.Id.Equals(Id), trackChanges).SingleOrDefault();

        public IEnumerable<Entities.Models.Task> GetTasksByProjectId(Guid projectId, 
            TaskRequestParameters requestParameters, 
            bool trackChanges) => 
            FindByCondition(x => x.ProjectId.Equals(projectId), trackChanges)
            .Filter(minPriotiry: requestParameters.MinPriority, maxPriority: requestParameters.MaxPriority)
            .Sort(requestParameters.OrderBy)
            .ToList();
    }
}
