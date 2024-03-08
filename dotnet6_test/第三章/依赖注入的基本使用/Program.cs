
using Microsoft.Extensions.DependencyInjection;

//先创建服务池实例
ServiceCollection sc = new ServiceCollection();
/*
往服务池里注册服务 共三种方式:1.AddTransient 瞬时注册 2.AddScoped 范围注册 3.AddSingleton 单例注册
*/
//sc.AddTransient<ITestService, TestServiceUSImpl>();
//sc.AddTransient<ITestService, TestServiceZHImpl>();
//sc.AddScoped<ITestService, TestServiceUSImpl>();
//sc.AddScoped<ITestService, TestServiceZHImpl>();
sc.AddSingleton<ITestService, TestServiceUSImpl>();
//sc.AddSingleton<ITestService, TestServiceZHImpl>();

//sc.AddSingleton(typeof(ITestService), () => new TestServiceUSImpl()); //特殊注册

//利用服务池建立服务提供者
using ServiceProvider sp = sc.BuildServiceProvider();
ITestService TestService = sp.GetService<ITestService>();
TestService.Name = "Jean";
TestService.SayHi();
ITestService TestService2 = sp.GetService<ITestService>();
Console.WriteLine(TestService == TestService2);
Console.WriteLine("TestService2:" + TestService2.Name);

