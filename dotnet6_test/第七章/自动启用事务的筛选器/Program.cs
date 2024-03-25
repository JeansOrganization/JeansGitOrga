using BookEFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 自动启用事务的筛选器;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<TransactionScopeFilter>();
});

builder.Services.AddDbContext<BookDbContext>(options =>
{
    options.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest", ServerVersion.Parse("8.2.0"));
});
builder.Services.AddDbContext<CarDbContext>(options =>
{
    options.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest", ServerVersion.Parse("8.2.0"));
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
