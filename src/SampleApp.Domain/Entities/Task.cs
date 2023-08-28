using SampleApp.Domain.Entities.Base;

namespace SampleApp.Domain.Entities
{
    public class Task : Entity
    {
        public Task(int id, string name, string description, DateTime dueDate, DateTime startDate, DateTime endDate,
            Enums.Priority priority, Enums.Status status)
        {
            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            StartDate = startDate;
            EndDate = endDate;
            Priority = priority;
            Status = status;
        }

        public Task(string name, string description, DateTime dueDate, DateTime startDate, DateTime endDate,
            Enums.Priority priority, Enums.Status status)
        {
            Name = name;
            Description = description;
            DueDate = dueDate;
            StartDate = startDate;
            EndDate = endDate;
            Priority = priority;
            Status = status;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Enums.Priority Priority { get; set; }
        public Enums.Status Status { get; set; }
    }
}
