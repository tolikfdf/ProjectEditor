using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProjectRepository : RepositoryBase<Entities.Models.Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateProject(Project project) =>
            Create(project);

        public void DeleteProject(Project project) => 
            Delete(project);

        public IEnumerable<Project> GetAllProjects(ProjectRequestParameters projectRequestParameters, bool trackChanges) => 
            FindAll(trackChanges).OrderBy(n => n.Name)
            .Filter(projectRequestParameters.MinPriority, 
                    projectRequestParameters.MaxPriority,
                    projectRequestParameters.MinStartDate,
                    projectRequestParameters.MaxStartDate,
                    projectRequestParameters.MinCompletionDate,
                    projectRequestParameters.MaxCompletionDate)
            .Sort(projectRequestParameters.OrderBy).ToList();

        public Project GetProjectById(Guid id, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault();
    }
}
