using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.Extensions.Configuration;
using 中间件入门案例;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.Map("/test", async pipeBuilder =>
{
    pipeBuilder.UseMiddleware<CheckMiddleware>();
    pipeBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("1-start</br>");
        await next.Invoke();
        await context.Response.WriteAsync("1-end</br>");
    });
    pipeBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("2-start</br>");
        await next.Invoke();
        await context.Response.WriteAsync("2-end</br>");
    });
    pipeBuilder.Run(async context =>
    {
        var obj = context.Items["BodyJson"];
        await context.Response.WriteAsync(obj + "Run</br>");
        
    });
});


app.Run();
