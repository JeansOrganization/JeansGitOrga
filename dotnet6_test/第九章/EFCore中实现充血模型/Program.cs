
using EFCore中实现充血模型;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ServiceCollection();
services.AddDbContext<MyDbContext>(options =>
    options.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest2",
            ServerVersion.Parse("8.2.0"))
);
using var provider = services.BuildServiceProvider();
using var dbContext = provider.GetRequiredService<MyDbContext>();

User user = new User("Jean");
user.ChangePassword("123456");
user.ChangeUserName("LILIQING");
dbContext.Users.Add(user);
await dbContext.SaveChangesAsync();

var u = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "LILIQING");
Console.WriteLine(u);