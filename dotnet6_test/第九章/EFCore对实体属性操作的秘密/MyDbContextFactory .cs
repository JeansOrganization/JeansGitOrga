using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore对实体属性操作的秘密
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest",
            ServerVersion.Parse("8.2.0"));

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
