using Microsoft.EntityFrameworkCore;
class MyDbContext : DbContext
{
    public DbSet<House> Houses { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=Jean;password=123456;database=jeanDb",
            new MySqlServerVersion(new Version(8, 6, 20)));
        optionsBuilder.LogTo(msg =>
        {
            if (msg.Contains("CommandExecuted"))
            {
                Console.WriteLine(msg);
            }
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}