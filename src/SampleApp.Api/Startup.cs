using SampleApp.Api.Extensions;
using SampleApp.Api.Filters;
using SampleApp.Api.HealthCheck;
using SampleApp.Api.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.Data.SqlClient;
using FluentValidation;

namespace SampleApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Service dependencies         
            services.ConfigureSampleAppServices(Configuration);

            // Redis Cache for Distributed Caching Purpose
            services.ConfigureRedisCache(Configuration);

            services.ConfigureApiVersioning();

            services.ConfigureCors();

            services.ConfigureSwagger();

            services.AddControllers(options =>
                {
                    // Bearer Token Authorization
                    // options.Filters.Add(typeof(CustomAuthorization));

                    options.Filters.Add<ValidationFilter>();
                })
                .WithPreventAutoValidation()
                .AddNewtonsoftJson();

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Startup>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Global Exception Handler Middleware
            app.UseApiExceptionHandler(options =>
            {
                options.AddResponseDetails = UpdateApiErrorResponse;
                options.DetermineLogLevel = DetermineLogLevel;
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1.0/swagger.json", "v1.0");
                //options.SwaggerEndpoint("/v2/swagger.json", "v2"); // Future Version
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultHealthChecks();
                endpoints.MapControllers();
            });
        }

        private static LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
                ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
            {
                return LogLevel.Critical;
            }

            return LogLevel.Error;
        }

        private static void UpdateApiErrorResponse(HttpContext context, Exception ex, Models.Response<ApiError> apiError)
        {
            if (ex.GetType().Name == nameof(SqlException))
            {
                apiError.Message = "Exception was a database exception!";
            }
        }
    }
}