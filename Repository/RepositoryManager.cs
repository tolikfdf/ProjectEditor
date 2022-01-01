using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext repositoryContext;
        private ITaskRepository taskRepository;
        private IProjectRepository projectRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public ITaskRepository Task
        {
            get
            {
                if (taskRepository == null)
                {
                    taskRepository = new TaskRepository(repositoryContext);
                }

                return taskRepository;
            }
        }

        public IProjectRepository Project
        {
            get
            {
                if (projectRepository == null)
                {
                    projectRepository = new ProjectRepository(repositoryContext);
                }

                return projectRepository;
            }
        }

        public void Save()
        {
            repositoryContext.SaveChanges();
        }
    }
}
