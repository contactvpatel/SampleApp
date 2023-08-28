using SampleApp.Domain;

namespace SampleApp.Api.Dto
{
    public class TaskCreateRequest
    {
        public TaskCreateRequest(string name, string description, DateTime dueDate, DateTime startDate,
            DateTime endDate,
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
