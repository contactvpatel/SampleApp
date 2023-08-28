using SampleApp.Api.Models;

namespace SampleApp.Api.Middleware
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, Response<ApiError>> AddResponseDetails { get; set; }

        public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
    }
}
