using SampleApp.Application.Models.Base;
using SampleApp.Domain;

namespace SampleApp.Application.Models
{
    public class TaskModel : BaseModel
    {
        public TaskModel(string name, string description, DateTime dueDate, DateTime startDate, DateTime endDate,
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
