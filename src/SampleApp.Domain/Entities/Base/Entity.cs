namespace SampleApp.Domain.Entities.Base
{
    public abstract class Entity
    {
        // Key Columns or Audit Tracking Columns (Created, CreatedBy, LastUpdated, LastUpdatedBy)
        public int Id { get; set; }
    }
}
