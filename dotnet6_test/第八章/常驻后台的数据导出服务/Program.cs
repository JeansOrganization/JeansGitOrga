using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using 常驻后台的数据导出服务;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<ExplortStatisticBgService>();
builder.Services.AddDataProtection();
builder.Services.AddDbContext<IdDbContext>(options =>
{
    options.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest", 
        ServerVersion.Parse("8.2.0"));
});
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});
var identityBuilder = new IdentityBuilder(typeof(User),typeof(Role),builder.Services);
identityBuilder.AddEntityFrameworkStores<IdDbContext>().AddDefaultTokenProviders()
    .AddUserManager<UserManager<User>>().AddRoleManager<RoleManager<Role>>();

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
