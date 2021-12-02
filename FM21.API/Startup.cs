using AutoMapper;
using FM21.API.Extensions;
using FM21.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FM21.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SetupAppConfigKeys();
            ServiceExtensions.ConfigureLoggerService(configuration);
        }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddMemoryCache();
            services.ConfigureIISIntegration();
            services.ConfigureRepositoryAndServices();
            services.SetupAPIVersioning();
            services.ConfigureControllerAndJsonSettings();
            //services.ConfigureLocalization();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.ConfigureSwagger();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IExceptionHandler exceptionHandler)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                app.WebWorker(context);
                await next.Invoke();
            });
            app.UseGlobalExceptionHandler(exceptionHandler);
            app.UseApiVersioning();
            app.UseHttpsRedirection();
            //app.AddLocalization();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.AddSwagger(env);
            exceptionHandler.LogInformation("API Started..");
        }

        /// <summary>
        /// Fectch and set global variables
        /// </summary>
        public static void SetupAppConfigKeys()
        {
            ApplicationConstants.EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            ApplicationConstants.DbConnectionString = Configuration.GetConnectionString("AppDBContextCon");
            ApplicationConstants.SQLServerTimeOut = Configuration.GetSection("AppSettings").GetValue<int>("SQLServerTimeOut");
            ApplicationConstants.EnableLog = Configuration.GetSection("AppSettings").GetValue<bool>("EnableLog");
            ApplicationConstants.LogFilePath = Configuration.GetSection("AppSettings").GetValue<string>("LogFilePath");
            ApplicationConstants.SupportedLocalization = Configuration.GetSection("AppSettings").GetValue<string>("Localization");
            ApplicationConstants.CacheDurationInSecond = Configuration.GetSection("AppSettings").GetValue<int>("CacheDurationInSecond");
            ApplicationConstants.Domain = Configuration.GetSection("AppSettings").GetValue<string>("Domain");
            ApplicationConstants.ADUserGroup = Configuration.GetSection("AppSettings").GetValue<string>("ADUserGroup");
        }
    }
}