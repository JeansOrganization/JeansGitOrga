using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolEFCore
{
    public class SchoolConfigFactory : IDesignTimeDbContextFactory<SchoolConfig>
    {
        public SchoolConfig CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
