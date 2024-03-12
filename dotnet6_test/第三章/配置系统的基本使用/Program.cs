
using Microsoft.Extensions.Configuration;
using 配置系统的基本使用;

/* 通过ConfigurationBuilder引入json配置文件创建配置根节点configRoot (需要引入Microsoft.Extensions.Configuration.Json包) */
/* AddJsonFile() 中optional:false表示文件不可选，如果找寻不到文件会报错，reloadOnChange表示更改后是否实时更新 */
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("config.json",optional:false,reloadOnChange:false);
IConfigurationRoot configRoot = configBuilder.Build();

/* 通过根节点获取某个子节点的数据 第一层configRoot["name"]  多层configRoot.GetSection("proxy:address") */
Console.WriteLine("\n------①------");
string name = configRoot["name"];
var address0 = configRoot["proxy:address"];
var address = configRoot.GetSection("proxy:address").Value; //多层规范写法
var port = configRoot.GetSection("proxy:port").Value;
Console.WriteLine(name + " " + address0 + " " + address + " " + port);

/* 将根节点获取到的某个对象节点绑定成对象 (需要引入Microsoft.Extensions.Configuration.Binder包) */
Console.WriteLine("\n------②------");
Config config = configRoot.GetSection("proxy").Get<Config>();
Console.WriteLine("config.address:" + config.address + "," +
    "config.port:" + config.port + "," +
    "config.username:" + config.username + "," +
    "config.password:" + config.password);



Console.Read();
