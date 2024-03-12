
using Microsoft.Extensions.Configuration;

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
var config = configBuilder.AddEnvironmentVariables(prefix: "jean_").Build();
string name = config["name"];
string address = config.GetSection("proxy:address").Value;
int[] ids = config.GetSection("proxy:ids").Get<int[]>();
Console.WriteLine($"name:{name},address:{address}");
foreach(int id in ids)
{
    Console.Write(id+",");
}
