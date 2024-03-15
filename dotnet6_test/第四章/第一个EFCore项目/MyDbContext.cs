using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql;

namespace 第一个EFCore项目
{
    public class MyDbContext : DbContext
    {
        private readonly StreamWriter _logStream = new StreamWriter("logs/EFCore.txt", append: true);
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = "server=127.0.0.1;port=3306;user=Jean;password=123456;database=jeandb2";
            optionsBuilder.UseMySql(connStr, ServerVersion.Parse("8.2.0"));//new MySqlServerVersion(new Version(8.2.0)));
            //optionsBuilder.LogTo(Console.WriteLine);
            /* Console.WriteLine、Debug.WriteLine */
            optionsBuilder.LogTo(msg => _logStream.WriteLine(msg.Replace("`","")) , 
                new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();

        }
    }
}
