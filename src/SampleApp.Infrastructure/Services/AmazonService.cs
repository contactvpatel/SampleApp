using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SampleApp.Domain.Models;
using SampleApp.Domain.Services;
using System.Net;

namespace SampleApp.Infrastructure.Services
{
    public class AmazonService : IAmazonService
    {
        private readonly RestClient _client;
        private readonly IOptionsMonitor<AmazonServiceSettingModel> _amazonServiceSettingModel;
        private readonly ILogger<AmazonService> _logger;

        public AmazonService(RestClient client, IOptionsMonitor<AmazonServiceSettingModel> targetServiceSettingModel, ILogger<AmazonService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _amazonServiceSettingModel = targetServiceSettingModel ?? throw new ArgumentNullException(nameof(targetServiceSettingModel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AmazonProductModel> GetProduct(string productId)
        {
            var apiUrl = $"{_amazonServiceSettingModel.CurrentValue.Url}/{_amazonServiceSettingModel.CurrentValue.Endpoint.Request}?api_key={_amazonServiceSettingModel.CurrentValue.Key}&type=product&asin={productId}&amazon_domain=amazon.com";
            var response = await Execute<AmazonProductResponse<AmazonProductModel>>(apiUrl);

            if (response == null)
            {
                RaiseNullResponseException();
                return null;
            }

            return response.Product;
        }

        private async Task<T> Execute<T>(string url)
        {
            var request = new RestRequest(url)
            {
                Method = Method.Get
            };
            request.AddHeader("Content-type", "application/json");

            var response = await _client.ExecuteAsync(request);

            if (response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NotFound)
                if (response.Content != null)
                    return JsonConvert.DeserializeObject<T>(response.Content);

            throw new ApplicationException(response.Content);
        }

        private void RaiseApplicationException<T>(AmazonProductResponse<T> response)
        {
            var errorMessage = response.Errors?.FirstOrDefault()?.ErrorId + "-" +
                               response.Errors?.FirstOrDefault()?.StatusCode + "-" +
                               response.Errors?.FirstOrDefault()?.Message;
            _logger.LogError(errorMessage);
            throw new ApplicationException(errorMessage);
        }

        private void RaiseNullResponseException()
        {
            const string errorMessage = "Received NULL response from Amazon Api.";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }
    }
}
