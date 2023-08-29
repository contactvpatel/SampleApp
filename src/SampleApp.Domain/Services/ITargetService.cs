using SampleApp.Domain.Models;

namespace SampleApp.Domain.Services
{
    public interface ITargetService
    {
        Task<TargetProductModel> GetProduct(string productId);
    }
}
