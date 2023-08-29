using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SampleApp.Domain.Models;
using SampleApp.Domain.Services;
using System.Net;

namespace SampleApp.Infrastructure.Services
{
    public class TargetService : ITargetService
    {
        private readonly RestClient _client;
        private readonly IOptionsMonitor<TargetServiceSettingModel> _targetServiceSettingModel;
        private readonly ILogger<TargetService> _logger;

        public TargetService(RestClient client, IOptionsMonitor<TargetServiceSettingModel> targetServiceSettingModel, ILogger<TargetService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _targetServiceSettingModel = targetServiceSettingModel ?? throw new ArgumentNullException(nameof(targetServiceSettingModel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TargetProductModel> GetProduct(string productId)
        {
            var apiUrl = $"{_targetServiceSettingModel.CurrentValue.Url}/{_targetServiceSettingModel.CurrentValue.Endpoint.Request}?api_key={_targetServiceSettingModel.CurrentValue.Key}&type=product&tcin={productId}";
            var response = await Execute<TargetProductResponse<TargetProductModel>>(apiUrl);

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

        private void RaiseApplicationException<T>(TargetProductResponse<T> response)
        {
            var errorMessage = response.Errors?.FirstOrDefault()?.ErrorId + "-" +
                               response.Errors?.FirstOrDefault()?.StatusCode + "-" +
                               response.Errors?.FirstOrDefault()?.Message;
            _logger.LogError(errorMessage);
            throw new ApplicationException(errorMessage);
        }

        private void RaiseNullResponseException()
        {
            const string errorMessage = "Received NULL response from Target Api.";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }
    }
}
