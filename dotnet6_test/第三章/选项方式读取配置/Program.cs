
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using 选项方式读取配置;


/* 获取配置根节点 */
var configRoot = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true).Build();


/* AddOptions() 注册服务IOptions<T>,IOptionsSnapshot<T>,IOptionsMonitor<T>,IOptionsFactory<T>,IOptionsMonitorCache<T> 
 (需引入Microsoft.Extensions.Options包)*/
/* Configure<T> 通过lambda表达式绑定配置对象(需引入Microsoft.Extensions.Configuration.Binder) */
ServiceCollection services = new ServiceCollection();
services.AddOptions()
    .Configure<DbSetting>(e => configRoot.GetSection("DB").Bind(e))
    .Configure<SmtpSetting>(e => configRoot.GetSection("Smtp").Bind(e));

services.AddScoped<Demo>();
using var provider = services.BuildServiceProvider();
while (true)
{
    var scope = provider.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    Demo demo = serviceProvider.GetService<Demo>();
    demo.test();
    Console.WriteLine("1结束");
    Console.ReadKey();
    demo.test();
    Console.WriteLine("2结束");
    Console.ReadKey();
}