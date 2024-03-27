using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace 常驻后台的数据导出服务
{
    public class IdDbContext : IdentityDbContext<User,Role,long>
    {
        public IdDbContext(DbContextOptions<IdDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
