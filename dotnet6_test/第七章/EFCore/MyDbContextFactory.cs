using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var builder =  new DbContextOptionsBuilder<MyDbContext>();
            string connStr = Environment.GetEnvironmentVariable("MySqlConnStr");
            string version = Environment.GetEnvironmentVariable("Version");
            builder.UseMySql("server=127.0.0.1;port=3306;user=Jean;password=123456;database=JeanTest", ServerVersion.Parse("8.2.0"));
            var myDbContext = new MyDbContext(builder.Options);
            return myDbContext;
        }
    }
}
