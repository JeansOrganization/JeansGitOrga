

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SY.Windows.Demo;
using SY.Windows.Test;

ServiceCollection services = new ServiceCollection();
services.AddLogging(logBuilder =>
{
    //logBuilder.AddConsole();
    logBuilder.AddNLog();
});
services.AddScoped<Test>();
services.AddScoped<Demo>();
using var provider = services.BuildServiceProvider();
var test = provider.GetRequiredService<Test>();
var demo = provider.GetRequiredService<Demo>();
for(int i = 0; i < 5; i++)
{
    test.test();
    demo.demo();
}