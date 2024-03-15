using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore原理
{
    public class MySqlContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = "server=127.0.0.1;port=3306;user=Jean;password=123456;database=jean03151354";
            optionsBuilder.UseMySql(connStr, new MySqlServerVersion(new Version(8, 2, 0)));
            //optionsBuilder.LogTo(msg =>
            //{
            //    if (msg.Contains("CommandExecuted"))
            //        Console.WriteLine(msg);
            //});
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
