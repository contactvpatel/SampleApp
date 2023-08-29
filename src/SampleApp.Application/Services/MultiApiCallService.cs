using SampleApp.Application.Interfaces;
using SampleApp.Application.Models;
using Microsoft.Extensions.Logging;
using SampleApp.Application.Mapper;
using SampleApp.Domain.Services;

namespace SampleApp.Application.Services
{
    public class MultiApiCallService : IMultiApiCallService
    {
        private readonly ITargetService _targetService;
        private readonly IAmazonService _amazonService;
        private readonly ILogger<MultiApiCallService> _logger;

        public MultiApiCallService(ITargetService targetService, IAmazonService amazonService, ILogger<MultiApiCallService> logger)
        {
            _targetService = targetService ?? throw new ArgumentNullException(nameof(targetService));
            _amazonService = amazonService ?? throw new ArgumentNullException(nameof(amazonService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<MultiApiCallModel> GetById(string targetProductId, string amazonProductId)
        {
            _logger.LogInformation($"Get Products. Target Product Id: {targetProductId}, Amazon Product Id: {amazonProductId}");

            // Using below code, we can call two api on seperate thread and wait until both calls completes before processing API Call result.
            // It will increase overall performance if we have enough core on machine to run parallel threads.

            var targetServiceTask = _targetService.GetProduct(targetProductId);
            var amazonServiceTask = _amazonService.GetProduct(amazonProductId);

            await Task.WhenAll(targetServiceTask, amazonServiceTask);

            var targetProduct = targetServiceTask.Result;
            var amazonProduct = amazonServiceTask.Result;


            // Alternate approach is to run one API at a time as shown below and I have added delays as you asked in requirement.
            //var targetProduct = await _targetService.GetProduct(targetProductId);

            //Thread.Sleep(2000); // 2 seconds delay

            //var amazonProduct = await _amazonService.GetProduct(amazonProductId);

            return new MultiApiCallModel
            {
                TargetProduct = ObjectMapper.Mapper.Map<TargetProductModel>(targetProduct),
                AmazonProduct = ObjectMapper.Mapper.Map<AmazonProductModel>(amazonProduct)
            };
        }
    }
}
