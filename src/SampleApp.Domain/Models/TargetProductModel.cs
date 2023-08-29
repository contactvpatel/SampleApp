using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SampleApp.Domain.Models
{
    public class TargetProductModel
    {
        public string Tcin { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Link { get; set; }
        public string Upc { get; set; }
        public string Dpci { get; set; }
    }

    public class TargetServiceSettingModel
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string AppSecret { get; set; }
        public TargetServiceEndpoint Endpoint { get; set; }
    }

    public class TargetServiceEndpoint
    {
        public string Request { get; set; }
    }

    public class TargetProductResponse<T>
    {
        public T Product { get; set; }
        public List<Error> Errors { get; set; }
    }
}
