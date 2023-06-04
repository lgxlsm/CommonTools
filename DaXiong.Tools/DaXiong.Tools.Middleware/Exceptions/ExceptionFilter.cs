using DaXiong.Tools.Middleware.Buffering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Tools.Middleware.Exceptions
{
    /// <summary>
    /// 捕获并处理控制器方法中的异常
    /// </summary>
    internal class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var parameters = context.HttpContext.GetHttpParameters();
            var requestInfo = $"ExceptionFilter[URL]:{context.HttpContext.Request.Path}-[Parameters]:{parameters}-[Begin]:{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
            _logger.LogError(context.Exception, requestInfo);

            if (context.ExceptionHandled == false)
            {
                string msg = context.Exception.Message;
                context.Result = new ContentResult
                {
                    Content = msg,
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "text/html;charset=utf-8"
                };
            }
            context.ExceptionHandled = true; //异常已处理了
            return Task.CompletedTask;

        }
    }
}
