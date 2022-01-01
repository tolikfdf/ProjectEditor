

using Entities.RequestFeatures;

namespace Contracts
{
    public interface ITaskRepository
    {
        IEnumerable<Entities.Models.Task> GetTasksByProjectId(Guid projectId, TaskRequestParameters requestParameters, bool trackChanges);
        Entities.Models.Task GetTaskById(Guid projectId, Guid Id, bool trackChanges);
        void CreateTask(Guid projectId, Entities.Models.Task taskEntity);

        void DeleteTask(Entities.Models.Task task);
    }
}
