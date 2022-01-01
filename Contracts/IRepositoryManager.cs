namespace Contracts
{
    public interface IRepositoryManager
    {
        ITaskRepository Task { get; }
        IProjectRepository Project { get; }
        void Save();
    }
}
