using Microsoft.AspNetCore.Mvc;
using TestService;
using 异常筛选器;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MvcOptions>(options =>
{
    //后注入的先执行
    options.Filters.Add<My2ExceptionFilter>();
    options.Filters.Add<MyExceptionFilter>();
});
builder.Services.AddScoped<DemoService>();

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
