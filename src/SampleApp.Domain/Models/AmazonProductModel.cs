namespace SampleApp.Domain.Models
{
    public class AmazonProductModel
    {
        public string Asin { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Link { get; set; }
    }

    public class AmazonServiceSettingModel
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string AppSecret { get; set; }
        public AmazonServiceEndpoint Endpoint { get; set; }
    }

    public class AmazonServiceEndpoint
    {
        public string Request { get; set; }
    }

    public class AmazonProductResponse<T>
    {
        public T Product { get; set; }
        public List<Error> Errors { get; set; }
    }
}