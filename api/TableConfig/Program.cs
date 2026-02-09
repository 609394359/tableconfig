
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using TableConfig.Services;
using TableConfig.Setup;
using TableConfig.Utils;
using Microsoft.AspNetCore.HttpOverrides;
using TableConfig.Global;
using System.Reflection;

namespace TableConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddNLog(new ConfigurationBuilder().AddJsonFile($"Config/NLog.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true).Build());
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
            builder.UseNLog();

            builder.Configuration.AddJsonFile($"Config/appsettings.{builder.Environment.EnvironmentName}.json");

            builder.WebHost.UseUrls(AppSettings.Configuration["Startup:ApiUrls"].Split(';'));

            builder.Services.AddMemoryCache();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(context =>
            {
                SetupAutofac.ConfigureContainer(context);
            });

            //跨域
            builder.Services.AddCorsSetup();
            //
            builder.Services.AddSqlsugarSetup();
            //扩展服务注册
            builder.Services.AddExtServiceSetup();

            // Add services to the container.
            builder.Services.AddControllers(o => {
                //全局异常过滤
                o.Filters.Add<GlobalActionMonitor>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                //抑制系统自带模型验证
                options.SuppressModelStateInvalidFilter = true;
            }).AddNewtonsoftJson(o => {
                SetupNewtonsoftJson.Setup(o);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerSetup();
            //响应压缩
            builder.Services.AddResponseCompression();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// 限制所处理的标头中的条目数
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For：保存代理链中关于发起请求的客户端和后续代理的信息。X-Forwarded-Proto：原方案的值 (HTTP/HTTPS)    
                options.KnownNetworks.Clear(); // 从中接受转接头的已知网络的地址范围。 使用无类别域际路由选择 (CIDR) 表示法提供 IP 范围。使用CDN时应清空
                options.KnownProxies.Clear(); // 从中接受转接头的已知代理的地址。 使用 KnownProxies 指定精确的 IP 地址匹配。使用CDN时应清空
            });

            var app = builder.Build();
            app.UseResponseCompression();

            // 跨域设置
            app.UseCorsSetup();
            // 说明文档
            app.UseSwaggerSetup();

            app.UseMiddleware<ExceptionMiddleware>();

            // 使用静态文件
            app.UseForwardedHeaders();
            // 使用静态文件
            app.UseStaticFiles();

            // 使用Routing
            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            // 使用cookie
            app.UseCookiePolicy();

            // 使用相应缓存中间件
            app.UseResponseCaching();

            app.MapControllers();

            app.Run();
        }
    }
}
