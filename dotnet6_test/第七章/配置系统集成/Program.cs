using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using ����ϵͳ����;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Host.ConfigureAppConfiguration((_, configure) =>
{
    //��ȡ���ڻ�����Ӧ��sql�����ַ���
    string connStr = builder.Configuration.GetConnectionString("MySql");
    //�������ݿⲢ��ȡ��Ӧ���õ�Configuration
    configure.AddDbConfiguration(() => new MySqlConnection(connStr));
});
/* �����ݿ��ȡ������ */
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
