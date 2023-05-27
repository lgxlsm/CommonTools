using DaXiong.Tools.Middleware.Buffering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Tools.Middleware.Exceptions
{
    /// <summary>
    /// 捕获并处理控制器方法中的异常
    /// </summary>
    internal class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var parameters = context.HttpContext.GetHttpParameters();
            var requestInfo = string.Format("[URL]:{0}-[Parameters]:{1}-[Begin]:{2}", context.HttpContext.Request.Path, parameters, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            _logger.LogError(context.Exception, requestInfo);

            context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            context.Result = new BadRequestObjectResult("系统繁忙，请稍后再试");
            //ExceptionHandled 属性为 true，那么异常仍然会被传递到全局异常处理程序（如中间件）中进行处理。
            context.ExceptionHandled = true;

        }
    }
}
