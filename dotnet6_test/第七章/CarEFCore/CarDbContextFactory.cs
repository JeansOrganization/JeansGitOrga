using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class CarDbContextFactory : IDesignTimeDbContextFactory<CarDbContext>
{
    public CarDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<CarDbContext>();
        builder.UseMySql("server=127.0.0.1;port=3306;user=Jean;password=123456;database=JeanTest",
            ServerVersion.Parse("8.2.0"));
        return new CarDbContext(builder.Options);
    }
}
