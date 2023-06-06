using DaXiong.Demo.WebApi.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DaXiong.Demo.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configurationBuilder.AddConfigurationFolder();
            Configuration = configurationBuilder.Build();
            var appSecret = Configuration.GetSection("AppConfig")["AppSecret"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var AppSecret = Configuration.GetSection(nameof(AppConfig)).GetSection(nameof(AppConfig.AppSecret)).Value;

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DaXiong.Demo.WebApi", Version = "v1" });
            });
            //services.AddException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // dev环境，一旦报错就会跳转到错误堆栈页面
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DaXiong.Demo.WebApi v1"));
            }
            else
            {
                // 非dev环境输出格式，个人根据实际情况扩展
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        // 处理异常并生成响应
                        var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                        var statusCode = (int)HttpStatusCode.InternalServerError;
                        var message = "An error occurred while processing your request.";

                        // 根据异常类型决定如何响应请求...

                        // 生成响应
                        context.Response.StatusCode = statusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            statusCode,
                            message
                        }));
                    });
                });
            }
    
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


    public class MyExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MyExceptionMiddleware> _logger;

        public MyExceptionMiddleware(RequestDelegate next, ILogger<MyExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred: {ex}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
