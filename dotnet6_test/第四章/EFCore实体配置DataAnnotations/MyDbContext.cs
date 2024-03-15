using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EFCore实体配置DataAnnotations
{
    public class MyDbContext : DbContext
    {
        public DbSet<Teacher> Teachers {  get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = "server=127.0.0.1;port=3306;user=Jean;password=123456;database=jeandb3";
            optionsBuilder.UseMySql(connStr, ServerVersion.Parse("8.2.0")) ;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
