using SampleApp.Domain.Repositories.Base;

namespace SampleApp.Domain.Repositories
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        Task<IEnumerable<Entities.Task>> GetActiveHighPriorityTasks(DateTime dueDate);
    }
}
