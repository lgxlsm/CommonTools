using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BufferingExtension
    {
        public static IServiceCollection AddBuffering(this IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                // 允许同步IO操作
                options.AllowSynchronousIO = true;
            });

            return services.AddSingleton<IStartupFilter, BufferingStartupFilter>();
        }
    }

    internal class BufferingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Use(next => context =>
                {
                    // 如果在处理请求正文之前没有调用 EnableBuffering() 方法，那么请求正文将只能被读取一次。但是如果启用了缓冲区，那么就可以多次读取请求正文数据了。
                    context.Request.EnableBuffering();
                    return next(context);
                });
                next(app);
            };
        }
    }

}
