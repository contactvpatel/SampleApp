using SampleApp.Domain;

namespace SampleApp.Api.Dto
{
    public class TaskResponseModel
    {
        public TaskResponseModel(int id, string name, string description, DateTime dueDate, DateTime startDate,
            DateTime endDate, Enums.Priority priority, Enums.Status status)
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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Enums.Priority Priority { get; set; }
        public Enums.Status Status { get; set; }
    }
}