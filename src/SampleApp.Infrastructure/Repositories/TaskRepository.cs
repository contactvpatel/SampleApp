using SampleApp.Domain;
using SampleApp.Domain.Repositories;
using SampleApp.Infrastructure.DbContext;
using SampleApp.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = SampleApp.Domain.Entities.Task;

namespace SampleApp.Infrastructure.Repositories
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        private readonly SampleAppContext _dbContext;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(SampleAppContext dbContext, ILogger<TaskRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Task>> GetActiveHighPriorityTasks(DateTime dueDate)
        {
            _logger.LogInformation("Get Active High Priority Tasks that are not finished yet.");
            return await _dbContext.Tasks
                .Where(x => x.DueDate.Date == dueDate.Date && x.Priority == Enums.Priority.High &&
                            x.Status != Enums.Status.Finished).ToListAsync();
        }
    }
}
