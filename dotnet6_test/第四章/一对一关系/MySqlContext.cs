
using Microsoft.EntityFrameworkCore;

public class MySqlContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "server=127.0.0.1;port=3306;user=Jean;password=123456;database=jeantestDb";
        optionsBuilder.UseMySql(connStr,new MySqlServerVersion(new Version(8,2,0)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}