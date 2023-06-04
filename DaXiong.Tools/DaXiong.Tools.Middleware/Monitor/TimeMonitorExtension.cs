using DaXiong.Tools.Middleware.Monitor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class TimeMonitorExtension
    {
        public static IServiceCollection AddTimeMonitor(this IServiceCollection services, Action<TimeMonitorOption> configure = null)
        {
            services.AddOptions<TimeMonitorOption>();

            services.PostConfigure<TimeMonitorOption>(x =>
            {
                configure?.Invoke(x);
            });

            services.AddBuffering();
            //使用 app.UseMiddleware<T>() 方法注册的中间件将不会被作为单例来管理。每当 ASP.NET Core 应用程序处理一个请求时，都会创建一个新的中间件实例。
            //因此，如果您需要实现一个需要在整个应用程序生命周期内只创建一次的中间件，建议使用 services.AddSingleton() 方法来注册该中间件。
            return services.AddSingleton<IStartupFilter, TimeMonitorStartupFilter>();
        }
    }

    internal class TimeMonitorStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<TimeMonitorMiddleware>();
                next(app);
            };
        }
    }
}
