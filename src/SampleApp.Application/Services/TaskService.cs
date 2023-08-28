using SampleApp.Application.Interfaces;
using SampleApp.Application.Mapper;
using SampleApp.Application.Models;
using SampleApp.Domain;
using SampleApp.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace SampleApp.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TaskModel>> Get()
        {
            _logger.LogInformation("Get All Tasks");
            var tasks = await _taskRepository.GetAllAsync();
            return ObjectMapper.Mapper.Map<IEnumerable<TaskModel>>(tasks);
        }

        public async Task<TaskModel> GetById(int id)
        {
            _logger.LogInformation($"Get Task By Id: {id}");
            var task = await _taskRepository.GetByIdAsync(id);
            return ObjectMapper.Mapper.Map<TaskModel>(task);
        }

        public async Task<TaskModel> Create(TaskModel taskModel)
        {
            var existingEntity = await _taskRepository.GetByIdAsync(taskModel.Id);
            if (existingEntity != null)
                throw new ApplicationException($"Task with id({taskModel.Id}) already exists");

            var mappedEntity = ObjectMapper.Mapper.Map<Domain.Entities.Task>(taskModel);
            if (mappedEntity == null)
                throw new ApplicationException("Task Model could not be mapped.");

            // System should not have more than 100 High Priority tasks which have the same due date and are not finished yet at any time
            if (taskModel.Priority == Enums.Priority.High)
            {
                var activeHighPriorityTasks = await _taskRepository.GetActiveHighPriorityTasks(mappedEntity.DueDate);
                if (activeHighPriorityTasks.Count() > 100)
                {
                    throw new ApplicationException(
                        $"More than 100 high priority tasks with due date ({mappedEntity.DueDate.ToShortDateString()}) are not finished yet hence new high priority task with same due date cannot be created.");
                }
            }

            var newEntity = await _taskRepository.AddAsync(mappedEntity);
            _logger.LogInformation($"Task is successfully created. Task Id : {newEntity.Id}.");

            var newMappedEntity = ObjectMapper.Mapper.Map<TaskModel>(newEntity);
            return newMappedEntity;
        }

        public async Task<TaskModel> Update(TaskModel taskModel)
        {
            var editTask = await _taskRepository.GetByIdAsync(taskModel.Id);
            if (editTask == null)
                throw new ApplicationException($"Task with id {taskModel.Id} doesn't exist.");

            // System should not have more than 100 High Priority tasks which have the same due date and are not finished yet at any time
            if (taskModel.Priority == Enums.Priority.High)
            {
                var activeHighPriorityTasks = await _taskRepository.GetActiveHighPriorityTasks(taskModel.DueDate);
                if (activeHighPriorityTasks.Count() > 100)
                {
                    throw new ApplicationException(
                        $"More than 100 high priority tasks with due date ({taskModel.DueDate.ToShortDateString()}) are not finished yet hence new high priority task with same due date cannot be created.");
                }
            }

            var mappedEntity = ObjectMapper.Mapper.Map<Domain.Entities.Task>(taskModel);
            await _taskRepository.UpdateAsync(mappedEntity);

            _logger.LogInformation($"Task is successfully updated. Task Id : {taskModel.Id}.");

            return taskModel;
        }

        public async Task Delete(int id)
        {
            var deleteTask = await _taskRepository.GetByIdAsync(id);
            if (deleteTask == null)
                throw new ApplicationException($"Task with id {id} doesn't exist.");

            await _taskRepository.DeleteAsync(deleteTask);
            _logger.LogInformation($"Task is successfully deleted. Task Id : {id}.");
        }
    }
}
