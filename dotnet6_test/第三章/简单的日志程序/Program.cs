
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SN.Windows.xxx;
using SY.Windows.xxx;


/* 获取sppsettings.json配置 */
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
var config = configBuilder.AddJsonFile("appsettings.json", false, true).Build();

ServiceCollection services = new ServiceCollection();
services.AddLogging(logBuilder =>
{
    /* 添加日志提供者 */
    logBuilder.AddConsole();
    /* 将Logging配置绑定到Logger中，就可以通过配置操作过滤器、日志等级 */
    logBuilder.AddConfiguration(config.GetSection("Logging"));
    ///* 设置允许展示的最小的日志等级 */
    //logBuilder.SetMinimumLevel(LogLevel.Trace);
    ///* provider校验日志提供者的名称，category校验日志发起方的命名空间，logLevel校验日志等级 */
    //logBuilder.AddFilter((provider, category, logLevel) =>
    //{
    //    if (provider.Contains("Console") &&
    //        category.Contains("Test") &&
    //        logLevel >= LogLevel.Information)
    //    {
    //        return true;
    //    }
    //    return false;
    //});
});
services.AddScoped<Test>();
services.AddScoped<Demo>();
using var provider = services.BuildServiceProvider();
var test = provider.GetRequiredService<Test>();
test.test();
var demo = provider.GetRequiredService<Demo>();
demo.demo();
