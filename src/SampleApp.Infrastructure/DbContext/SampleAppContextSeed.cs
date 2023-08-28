using SampleApp.Domain;
using Microsoft.Extensions.Logging;

namespace SampleApp.Infrastructure.DbContext
{
    public class SampleAppContextSeed
    {
        public static async Task SeedAsync(SampleAppContext sampleAppContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            if (retry != null)
            {
                var retryForAvailability = retry.Value;

                try
                {
                    // TODO: Only run this if using a real database
                    // sampleAppContext.Database.Migrate();
                    // sampleAppContext.Database.EnsureCreated();

                    if (!sampleAppContext.Tasks.Any())
                    {
                        sampleAppContext.Tasks.AddRange(GetPreconfiguredTasks());
                        await sampleAppContext.SaveChangesAsync();
                    }
                }
                catch (Exception exception)
                {
                    if (retryForAvailability < 10)
                    {
                        retryForAvailability++;
                        var log = loggerFactory.CreateLogger<SampleAppContextSeed>();
                        log.LogError(exception.Message);
                        await SeedAsync(sampleAppContext, loggerFactory, retryForAvailability);
                    }

                    throw;
                }
            }
        }

        private static IEnumerable<Domain.Entities.Task> GetPreconfiguredTasks()
        {
            return new List<Domain.Entities.Task>
            {
                new("Task 1", "Task 1 Description", DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(-2),
                    DateTime.Now.AddDays(-1), Enums.Priority.High, Enums.Status.Finished),
                new("Task 2", "Task 2 Description", DateTime.Now.AddDays(3),
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddDays(1), Enums.Priority.Middle, Enums.Status.InProgress),
                new("Task 3", "Task 3 Description", DateTime.Now.AddDays(5),
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(2), Enums.Priority.Low, Enums.Status.New),
                new("Task 4", "Task 4 Description", DateTime.Now.AddDays(5),
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(3), Enums.Priority.High, Enums.Status.New),
                new("Task 5", "Task 5 Description", DateTime.Now.AddDays(5),
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(4), Enums.Priority.Middle, Enums.Status.New)
            };
        }
    }
}
