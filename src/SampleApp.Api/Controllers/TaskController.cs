using AutoMapper;
using SampleApp.Api.Dto;
using SampleApp.Api.Models;
using SampleApp.Application.Interfaces;
using SampleApp.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Api.Controllers
{
    [Route("api/v{version:apiVersion}/tasks")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger, IMapper mapper)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseModel>>> Get()
        {
            _logger.LogInformation("Get All Tasks");
            var tasks = await _taskService.Get();

            var message = $"Found {tasks.Count()} tasks";
            _logger.LogInformation(message);

            return Ok(new Response<IEnumerable<TaskResponseModel>>(_mapper.Map<IEnumerable<TaskResponseModel>>(tasks),
                true,
                message));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskResponseModel>> GetById(int id)
        {
            _logger.LogInformation($"Get Task By Id: {id}");

            var task = await _taskService.GetById(id);

            if (task != null) return Ok(new Response<TaskResponseModel>(_mapper.Map<TaskResponseModel>(task)));

            var message = $"No task found with id {id}";
            _logger.LogInformation(message);
            return NotFound(new Response<TaskResponseModel>(null, false, message));
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseModel>> Post([FromBody] TaskCreateRequest taskCreateRequest)
        {
            var newTask = await _taskService.Create(_mapper.Map<TaskModel>(taskCreateRequest));

            _logger.LogInformation($"Created New Task. Id: {newTask.Id}, Name: {newTask.Name}");

            return Ok(new Response<TaskResponseModel>(_mapper.Map<TaskResponseModel>(newTask)));
        }

        [HttpPut]
        public async Task<ActionResult<TaskResponseModel>> Put([FromBody] TaskUpdateRequest taskUpdateRequest)
        {
            var updatedTask = await _taskService.Update(_mapper.Map<TaskModel>(taskUpdateRequest));

            _logger.LogInformation($"Updated Task. Id: {updatedTask.Id}, Name: {updatedTask.Name}");

            return Ok(new Response<TaskResponseModel>(_mapper.Map<TaskResponseModel>(updatedTask)));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<TaskResponseModel>> Delete(int id)
        {
            await _taskService.Delete(id);

            _logger.LogInformation($"Deleted Task By Id: {id}");

            return Ok(new Response<TaskResponseModel>(null));
        }
    }
}
