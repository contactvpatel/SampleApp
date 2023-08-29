using SampleApp.Api.Extensions.Swagger;
using SampleApp.Application.Interfaces;
using SampleApp.Application.Services;
using SampleApp.Domain.Models;
using SampleApp.Domain.Repositories;
using SampleApp.Domain.Repositories.Base;
using SampleApp.Infrastructure.DbContext;
using SampleApp.Infrastructure.Repositories;
using SampleApp.Infrastructure.Repositories.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SampleApp.Domain.Services;
using SampleApp.Infrastructure.Services;
using RestSharp;

namespace SampleApp.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSampleAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Database
            ConfigureDatabases(services, configuration);

            // Add Infrastructure Layer
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITargetService, TargetService>();
            services.AddScoped<IAmazonService, AmazonService>();

            // Add Application Layer
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IMultiApiCallService, MultiApiCallService>();

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //External Service Dependency (Example: TargetService, AmazonService)
            services.AddScoped<RestClient>();
            services.Configure<TargetServiceSettingModel>(configuration.GetSection("TargetService"));
            services.Configure<AmazonServiceSettingModel>(configuration.GetSection("AmazonService"));

            // HealthChecks
            services.AddHealthChecks().AddDbContextCheck<SampleAppContext>();
        }

        private static void ConfigureDatabases(IServiceCollection services, IConfiguration configuration)
        {
            var databaseConnectionSettings = new DbConnectionModel();
            configuration.GetSection("DbConnectionSettings").Bind(databaseConnectionSettings);

            if (databaseConnectionSettings.UseInMemoryDatabase)
            {
                services.AddDbContext<SampleAppContext>(c =>
                    c.UseInMemoryDatabase("SampleApp").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }
            else
            {
                services.AddDbContextPool<SampleAppContext>(c =>
                    c.UseSqlServer(CreateConnectionString(databaseConnectionSettings),
                        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery).EnableRetryOnFailure(
                            maxRetryCount: 4,
                            maxRetryDelay: TimeSpan.FromSeconds(1),
                            errorNumbersToAdd: new int[] { }
                        )));
            }
        }

        private static string CreateConnectionString(DbConnectionModel databaseConnectionModel)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = string.IsNullOrEmpty(databaseConnectionModel.Port)
                    ? databaseConnectionModel.Host
                    : databaseConnectionModel.Host + "," + databaseConnectionModel.Port,
                InitialCatalog = databaseConnectionModel.DatabaseName
            };

            if (databaseConnectionModel.IntegratedSecurity)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = databaseConnectionModel.UserName;
                builder.Password = databaseConnectionModel.Password;
            }

            return builder.ConnectionString;
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                // Supporting multiple versioning scheme
                // Route (api/v1/accounts)
                // Header (X-version=1)
                // Querystring (api/accounts?api-version=1)
                // Media Type (application/json;v=1)
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-version"), new QueryStringApiVersionReader("api-version"),
                    new MediaTypeApiVersionReader("v"));
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(ConfigureSwaggerGen);
        }

        private static void ConfigureSwaggerGen(SwaggerGenOptions options)
        {
            AddSwaggerDocs(options);

            options.OperationFilter<RemoveVersionFromParameter>();
            options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            options.DocInclusionPredicate((version, desc) =>
            {
                if (!desc.TryGetMethodInfo(out var methodInfo))
                    return false;

                var versions = methodInfo
                    .DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                var maps = methodInfo
                    .GetCustomAttributes(true)
                    .OfType<MapToApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToList();

                return versions?.Any(v => $"v{v}" == version) == true
                       && (!maps.Any() || maps.Any(v => $"v{v}" == version));
            });

            // Add JWT Authentication

            /*
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
            */
        }

        private static void AddSwaggerDocs(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1.0", new OpenApiInfo
            {
                Version = "v1.0",
                Title = "Task API"
            });

            // Future Version
            //options.SwaggerDoc("v2", new OpenApiInfo
            //{
            //    Version = "v2",
            //    Title = "Task API"
            //});
        }

        public static void ConfigureRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheModel();

            configuration.GetSection("RedisCacheSettings").Bind(redisCacheSettings);

            services.AddSingleton(redisCacheSettings);

            if (redisCacheSettings.Enabled)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisCacheSettings.ConnectionString;
                    options.InstanceName = redisCacheSettings.InstanceName;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.AddSingleton<IRedisCacheService, RedisCacheService>();
        }
    }
}
