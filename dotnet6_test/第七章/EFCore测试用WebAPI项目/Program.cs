using EFCore;
using EFCore²âÊÔÓÃWebAPIÏîÄ¿;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zack.Commons;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
builder.Services.AddAllDbContexts(options =>
{
    string connStr = builder.Configuration.GetConnectionString("MySqlConnStr");
    string version = builder.Configuration.GetConnectionString("Version");
    options.UseMySql(connStr, ServerVersion.Parse(version));
} ,assemblies);

//builder.Services.AddDbContext<MyDbContext>(options =>
//{
//    string connStr = builder.Configuration.GetConnectionString("MySqlConnStr");
//    string version = builder.Configuration.GetConnectionString("Version");
//    options.UseMySql(connStr, ServerVersion.Parse(version));
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
