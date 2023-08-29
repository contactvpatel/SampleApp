using SampleApp.Domain.Models;

namespace SampleApp.Domain.Services
{
    public interface IAmazonService
    {
        Task<AmazonProductModel> GetProduct(string productId);
    }
}
