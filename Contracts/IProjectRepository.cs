using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAllProjects(ProjectRequestParameters projectRequestParameters, bool trackChanges);
        Project GetProjectById(Guid id, bool trackChanges);
        void CreateProject(Project project);
        void DeleteProject(Project project);
    }
}
