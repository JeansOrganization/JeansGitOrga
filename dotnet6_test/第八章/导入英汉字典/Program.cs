using µ¼ÈëÓ¢ºº×Öµä;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConnStrOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddScoped<ImportExecutor>();
builder.Services.AddSignalR();
string[] urls = new string[] { "http://localhost:8080" };
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
    .WithOrigins(urls)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<ImportDictHub>("/Hub/ImportDictHub");
app.MapControllers();

app.Run();
