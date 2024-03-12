using Microsoft.Extensions.Configuration;

ConfigurationBuilder configBuilder = new ConfigurationBuilder();

var config = configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables("jean_").Build();

string server = config["server"];
string username = config["username"];
string password = config["password"];
string name = config["name"];
string address = config["address"];
int[] ids = config.GetSection("proxy:ids").Get<int[]>();

Console.WriteLine($"server:{server},username:{username},password:{password},name:{name},address:{address}");
Console.WriteLine();
Console.Write("ids:[");
foreach (int id in ids)
{
    Console.Write(id);
    if(id != ids.Last()) Console.Write(",");
}
Console.Write("]");