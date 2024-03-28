using SignalR基本使用;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
    

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
app.UseCors(opt =>
{
    //不要允许所有源，因为这样编译会报错
    opt.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});
app.MapHub<CharHub>("/Hub/CharHub");


app.Run();
