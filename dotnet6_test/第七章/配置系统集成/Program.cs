using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using 配置系统集成;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Host.ConfigureAppConfiguration((_, configure) =>
{
    //获取所在环境对应的sql连接字符串
    string connStr = builder.Configuration.GetConnectionString("MySql");
    //连接数据库并获取相应配置到Configuration
    configure.AddDbConfiguration(() => new MySqlConnection(connStr));
});
/* 从数据库获取的配置 */
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:configuration").Value;
    options.InstanceName = builder.Configuration.GetSection("Redis:instanceName").Value;
});
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));
builder.Services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();
builder.Services.AddScoped<BookService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
