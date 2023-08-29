using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Api.Dto;
using SampleApp.Api.Models;
using SampleApp.Application.Interfaces;

namespace SampleApp.Api.Controllers
{
    [Route("api/v{version:apiVersion}/multi-api-call")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MultiApiCallController : ControllerBase
    {
        private readonly IMultiApiCallService _multiApiCallService;
        private readonly ILogger<MultiApiCallController> _logger;
        private readonly IMapper _mapper;

        public MultiApiCallController(IMultiApiCallService multiApiCallService, ILogger<MultiApiCallController> logger, IMapper mapper)
        {
            _multiApiCallService = multiApiCallService ?? throw new ArgumentNullException(nameof(multiApiCallService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<TaskResponseModel>> GetById(string targetProductId, string amazonProducId)
        {
            _logger.LogInformation($"Get Products By Id. Target Product Id: {targetProductId}, Amazon Product Id: {amazonProducId}");

            var products = await _multiApiCallService.GetById(targetProductId, amazonProducId);

            if (products != null) return Ok(new Response<MultiApiCallResponseModel>(_mapper.Map<MultiApiCallResponseModel>(products)));

            var message = $"No products found.";
            _logger.LogInformation(message);
            return NotFound(new Response<MultiApiCallResponseModel>(null, false, message));
        }
    }
}
