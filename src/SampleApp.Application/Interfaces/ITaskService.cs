using SampleApp.Application.Models;

namespace SampleApp.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskModel>> Get();
        Task<TaskModel> GetById(int id);
        Task<TaskModel> Create(TaskModel taskModel);
        Task<TaskModel> Update(TaskModel taskModel);
        Task Delete(int id);
    }
}
