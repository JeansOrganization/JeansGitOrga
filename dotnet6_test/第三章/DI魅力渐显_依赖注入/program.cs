
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;



ServiceCollection serviceCollection = new ServiceCollection();
//serviceCollection.AddScoped(typeof(IDbConnection),sp =>
serviceCollection.AddScoped<IDbConnection>(sp =>
{
    string connString = "server=127.0.0.1;port=3306;user=Jean;password=123456;database=jeandb";
    var conn = new MySqlConnection(connString);
    conn.Open();
    return conn;
});
serviceCollection.AddScoped<IUserBiz, UserBiz>();
serviceCollection.AddScoped<IUserDao, UserDao>();


using var serviceProvider = serviceCollection.BuildServiceProvider();

IUserBiz userBiz = serviceProvider.GetService<IUserBiz>();
bool isOk = userBiz.CheckLogin("17677369347", "123456", out string msg);
Console.WriteLine("isOK:" + isOk + ", " + msg);


