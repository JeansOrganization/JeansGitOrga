using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore中发布领域事件的合适时机
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest3",
            ServerVersion.Parse("8.2.0"));

            return new UserDbContext(optionsBuilder.Options, null);
        }
    }
}
