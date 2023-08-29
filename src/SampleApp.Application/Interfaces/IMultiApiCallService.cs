using SampleApp.Application.Models;

namespace SampleApp.Application.Interfaces
{
    public interface IMultiApiCallService
    {
        Task<MultiApiCallModel> GetById(string targetProductId, string amazonProductId);
    }
}
