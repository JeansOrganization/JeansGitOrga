using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nlog.AspNetCore;
using Nlog.WebApiDemo.Filters;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<INLogHelper, NLogHelper>();

builder.Logging.AddNLog("nlog.config");
//builder.Services.AddLogging(config =>
//{
//    config.AddNLog("nlog.config");
//});
builder.Services.Configure<MvcOptions>(config =>
{
    config.Filters.Add<CustomerGlobalExceptionFilterAsync>();
});

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
