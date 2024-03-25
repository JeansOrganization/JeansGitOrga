using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEFCore
{
    public class BookDbContextFactory : IDesignTimeDbContextFactory<BookDbContext>
    {
        public BookDbContext CreateDbContext(string[] args)
        {
            var builder =  new DbContextOptionsBuilder<BookDbContext>();
            builder.UseMySql("server=127.0.0.1;port=3306;user=Jean;password=123456;database=JeanTest", ServerVersion.Parse("8.2.0"));
            var myDbContext = new BookDbContext(builder.Options);
            return myDbContext;
        }
    }
}
