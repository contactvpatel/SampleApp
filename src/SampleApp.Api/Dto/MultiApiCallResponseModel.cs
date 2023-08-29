using SampleApp.Api.Models;

namespace SampleApp.Api.Dto
{
    public class MultiApiCallResponseModel
    {
        public MultiApiCallResponseModel()
        {
            TargetProduct = new TargetProductModel();
            AmazonProduct = new AmazonProductModel();
        }

        public TargetProductModel TargetProduct { get; set; }
        public AmazonProductModel AmazonProduct { get; set; }
    }
}