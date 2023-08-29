using System.Net.Http;
using System.Threading.Tasks;
using SampleApp.Api.Dto;
using SampleApp.Api.Models;
using Newtonsoft.Json;
using Xunit;

namespace SampleApp.Api.Tests.Controllers
{
    public class MultiApiCallControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private HttpClient Client { get; }

        public MultiApiCallControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        [Theory]
        [InlineData("78025470", "B07WJJF8PB")]
        public async Task GetTasksByIdTest(string targetProductId, string amazonProducId)
        {
            // Arrange & Act
            var response = await Client.GetAsync($"/api/v1.0/multi-api-call/?targetProductId={targetProductId}&amazonProducId={amazonProducId}");
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<Response<MultiApiCallResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(result is { Succeeded: true });
            Assert.NotNull(result.Data);
            Assert.Equal(targetProductId, result.Data.TargetProduct.Tcin);
            Assert.Equal(amazonProducId, result.Data.AmazonProduct.Asin);
            Assert.Null(result.Errors);
        }
    }
}