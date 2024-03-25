using CarEFCore;
using Microsoft.EntityFrameworkCore;

public class CarDbContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public CarDbContext(DbContextOptions<CarDbContext> options):base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
