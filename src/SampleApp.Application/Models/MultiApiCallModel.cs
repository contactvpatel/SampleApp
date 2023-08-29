namespace SampleApp.Application.Models
{
    public class MultiApiCallModel
    {
        public MultiApiCallModel()
        {
            TargetProduct = new TargetProductModel();
            AmazonProduct = new AmazonProductModel();
        }

        public TargetProductModel TargetProduct { get; set; }
        public AmazonProductModel AmazonProduct { get; set; }
    }
}
