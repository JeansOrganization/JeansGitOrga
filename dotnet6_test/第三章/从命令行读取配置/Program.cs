
using Microsoft.Extensions.Configuration;

/* 在项目根目录启动命令行,输入***.exe key=value key1:key2=value2 */
/* 从命令行读取配置.exe server=127.0.0.1 proxy:username=jean
    输出:server:127.0.0.1,username:jean */
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
var config = configBuilder.AddCommandLine(args).Build();
var server = config["server"];
var username = config.GetSection("proxy:username").Value;
Console.WriteLine($"server:{server},username:{username}");
