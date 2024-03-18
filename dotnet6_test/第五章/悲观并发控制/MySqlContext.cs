using Microsoft.EntityFrameworkCore;

public class MySqlContext : DbContext
{
    public DbSet<House> Houses {  get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "server=127.0.0.1;user=Jean;password=123456;database=jeanDb";
        optionsBuilder.UseMySql(connStr,new MySqlServerVersion(new Version(8,2,0)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
