using FM21.Core;
using FM21.Data;
using FM21.Data.Infrastructure;
using FM21.Service;
using FM21.Service.Caching;
using FM21.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace FM21.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", policyBuilder =>
            {
                policyBuilder
                    .WithOrigins().AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });
        }

        public static void SetupAPIVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.UseApiBehavior = true;
                //options.ApiVersionReader = new HeaderApiVersionReader("version");
            });
        }

        public static void ConfigureControllerAndJsonSettings(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }

        public static void ConfigureDBContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppEntities>(options => options.UseSqlServer(ApplicationConstants.DbConnectionString));
        }

        public static void ConfigureRepositoryAndServices(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<ICacheProvider, CacheProvider>();
            services.Add(new ServiceDescriptor(typeof(IRepository<>), typeof(Repository<>), ServiceLifetime.Scoped));
            services.AddSingleton<IExceptionHandler, ExceptionHandler>();

            #region Services
            services.AddScoped<IAllergenMasterService, AllergenMasterService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IFormulaMasterService, FormulaMasterService>();
            services.AddScoped<IIngredientMasterService, IngredientMasterService>();
            services.AddScoped<IInstructionMasterService, InstructionMasterService>();
            services.AddScoped<IInstructionGroupMasterService, InstructionGroupMasterService>();
            services.AddScoped<IPermissionMasterService, PermissionMasterService>();
            services.AddScoped<IProjectMasterService, ProjectMasterService>();
            services.AddScoped<IRoleMasterService, RoleMasterService>();
            services.AddScoped<ISupplierMasterService, SupplierMasterService>();
            services.AddScoped<IRegulatoryMasterService, RegulatoryMasterService>();
            services.AddScoped<IUserMasterService, UserMasterService>();
            #endregion
        }

        public static void ConfigureLoggerService(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                            .Filter.ByExcluding(_ => !ApplicationConstants.EnableLog)
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.File(
                                ApplicationConstants.LogFilePath,
                                fileSizeLimitBytes: 1_000_000,
                                rollingInterval: RollingInterval.Day,
                                rollOnFileSizeLimit: true,
                                shared: true,
                                flushToDiskInterval: TimeSpan.FromSeconds(1)
                            )
                            .CreateLogger();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"FM21 API ({ApplicationConstants.EnvironmentName})", Version = "v1" });
                c.OperationFilter<SwaggerHeaderFilter>();
                c.EnableAnnotations();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        public static void AddSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = "api";
                c.DocumentTitle = $"FM21 API: {env?.EnvironmentName}";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FM21 API v1");
                c.DefaultModelExpandDepth(2);
                //c.DefaultModelRendering(ModelRendering.Example);
                //c.DefaultModelsExpandDepth(-1);
                //c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableFilter();
                //c.ShowExtensions();
                c.EnableValidator();
            });
        }
    }
}