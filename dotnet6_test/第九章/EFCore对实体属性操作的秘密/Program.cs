
using EFCore对实体属性操作的秘密;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ServiceCollection();
services.AddDbContext<MyDbContext>(options =>
    options.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest2",
            ServerVersion.Parse("8.2.0"))
);
using var provider = services.BuildServiceProvider();
using var dbContext = provider.GetRequiredService<MyDbContext>();


Dog dog = new Dog() { Name = "旺财" };
Console.WriteLine("Dog初始化完毕");
dbContext.Dogs.Add(dog);
await dbContext.SaveChangesAsync();
Console.WriteLine("SaveChangesAsync完毕");

Console.WriteLine("准备读取数据");
Dog? dog2 = await dbContext.Dogs.FirstOrDefaultAsync(x => x.Name == "旺财");
Console.WriteLine("读取数据完毕");
