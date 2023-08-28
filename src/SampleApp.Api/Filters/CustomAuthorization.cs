using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleApp.Api.Filters
{
    public class CustomAuthorization : IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthorization(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>  
        /// Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {
            // Sample Custom Authorization that intercept API calls, get Bearer Token, call SSO Service to validate token and accordingly process request

            /*
            if (filterContext == null) return;

            var hasAllowAnonymous = filterContext.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute));

            if (hasAllowAnonymous) return;

            filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var authTokens);

            var token = authTokens.FirstOrDefault();

            token = token?.Replace("Bearer ", "");

            if (token != null)
            {
                if (await _ssoService.ValidateToken(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var tokenInformation = handler.ReadToken(token) as JwtSecurityToken;
                    _httpContextAccessor.HttpContext?.Request.HttpContext.Items.Add("UserId",
                        (tokenInformation?.Claims ?? Array.Empty<Claim>()).FirstOrDefault(x => x.Type == "pid")
                        ?.Value);
                    _httpContextAccessor.HttpContext?.Request.HttpContext.Items.Add("UserToken", token);
                }
                else
                {
                    //_logger.LogUnauthorizedAccess("Authentication", "Invalid Token");
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.Result = new UnauthorizedResult();
                }
            }
            else
            {
                //_logger.LogUnauthorizedAccess("Authentication", "Token is NULL");
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new UnauthorizedResult();
            }
            */
        }
    }
}
