namespace SampleApp.Domain.Models
{
    public class DbConnectionModel
    {
        public bool UseInMemoryDatabase { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string DatabaseName { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
