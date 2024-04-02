
using EFCore中实现值对象;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;


ServiceCollection services = new ServiceCollection();
services.AddDbContext<MyDbContext>(options =>
    options.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest2",
            ServerVersion.Parse("8.2.0"))
);
using var provider = services.BuildServiceProvider();
using var dbContext = provider.GetRequiredService<MyDbContext>();

//Region region = new Region(
//    new MultilingualString("陆丰", "lufeng"),
//    new Area(3000, AreaUnitType.SquareKM),
//    new Geo(134.52, 85.63),
//    RegionLevel.Town);
//dbContext.Regions.Add(region);
//await dbContext.SaveChangesAsync();

//var region2 = await dbContext.Regions.FirstOrDefaultAsync(r => r.Name == new MultilingualString("陆丰", "lufeng"));
var region2 = await dbContext.Regions.FirstOrDefaultAsync(
    ExpressionHelper.MakeEqual<Region, MultilingualString>(r=>r.Name, new MultilingualString("陆丰", "lufeng")));
//var region2 = await dbContext.Regions.FirstOrDefaultAsync(r => r.Name.Chinese == "陆丰" && r.Name.English == "lufeng");
Console.WriteLine(JsonConvert.SerializeObject(region2));